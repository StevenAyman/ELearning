using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings;
using ELearning.Domain.Ratings.Events;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Students;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ELearning.Domain.Tests.Unit.Ratings;
public sealed class SessionRatingDomainServiceTests
{
    private readonly SessionRatingDomainService _sut;
    private readonly IPurchaseRepository _purchaseRepo = Substitute.For<IPurchaseRepository>();

    public SessionRatingDomainServiceTests()
    {
        _sut = new SessionRatingDomainService(_purchaseRepo);
    }

    [Fact]
    public async Task RateSession_ShouldReturnPurchaseNotFoundError_WhenStudentTryToRateUnPurchasedSession()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string sessionId = "1", studentId = "2";
        var rating = Rating.CreateRating(5);
        var utc = DateTime.UtcNow;
        _purchaseRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
                .ReturnsNull();

        // Act
        var result = await _sut.RateSession(sessionId, studentId, rating, utc, token);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PurchaseErrors.PurchaseNotFound);
    }

    [Fact]
    public async Task RateSession_ShouldReturnCreateSessionRating_WhenStudentWasPurchased()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string sessionId = "1", studentId = "2";
        var rating = Rating.CreateRating(5);
        var utc = DateTime.UtcNow;
        _purchaseRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
                .Returns(Purchase.CreateSessionPurchase("33", "2", "1", utc));
        var sessionRatings = SessionsRating.CreateNewRating(sessionId, studentId, rating, utc);

        // Act
        var result = await _sut.RateSession(sessionId, studentId, rating, utc, token);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().BeEquivalentTo(sessionRatings, options => 
        options.Excluding(sr => sr.DomainEvents)
        .Excluding(sr => sr.Id));
    }

    [Fact]
    public async Task RateSession_ShouldRaiseSessionRatedDomainEvent_WhenRatingDoneSuccessfully()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string sessionId = "1", studentId = "2";
        var rating = Rating.CreateRating(5);
        var utc = DateTime.UtcNow;
        _purchaseRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
                .Returns(Purchase.CreateSessionPurchase("33", "2", "1", utc));

        // Act
        var result = await _sut.RateSession(sessionId, studentId, rating, utc, token);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Value.DomainEvents.Should().HaveCount(1);
        result.Value.DomainEvents[0].Should().Be(new SessionRatedDomainEvent(sessionId, rating));
    }
}
