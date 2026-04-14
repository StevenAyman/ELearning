using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings;
using ELearning.Domain.Ratings.Events;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Students;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ELearning.Domain.Tests.Unit.Ratings;
public sealed class InstructorRatingDomainServiceTests
{
    private readonly InstructorRatingDomainService _sut;
    private readonly IInstructorsRatingRepository _instructorRatingRepo = Substitute.For<IInstructorsRatingRepository>();
    private readonly IPurchaseRepository _purchaseRepo = Substitute.For<IPurchaseRepository>();
    private readonly ISessionRepository _sessionRepo = Substitute.For<ISessionRepository>();
    
    public InstructorRatingDomainServiceTests()
    {
        _sut = new InstructorRatingDomainService(_purchaseRepo, _sessionRepo, _instructorRatingRepo);
    }

    [Fact]
    public async Task RateInstructor_ShouldThrowException_WhenTheStudentAlreadyRatedInstructorBefore()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string instructorId = "1", studentId = "2";
        var instructorRating  = InstructorsRating.CreateNewRating("1", "2", Rating.CreateRating(4), DateTime.UtcNow);
        _instructorRatingRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<InstructorsRating>>(), token)
            .Returns(instructorRating);

        // Act
        var result =  async () => await _sut.RateInstructor(instructorId, studentId, Rating.CreateRating(5), DateTime.UtcNow, token);

        // Assert
        await result.Should().ThrowAsync<ApplicationException>()
            .WithMessage("You should update the rating not creating new rating");
    }

    [Fact]
    public async Task RateInstructor_ShouldReturnPurchaseNotFoundError_WhenTheStudentDidntPurchaseAnySessionsForInstructor()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;  
        string instructorId = "1", studentId = "2";
        _instructorRatingRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<InstructorsRating>>(), token)
            .ReturnsNull();
        _purchaseRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
            .Returns(new List<Purchase>());
        _sessionRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Session>>(), token)
            .Returns(new List<Session>());

        var exprected = Result<InstructorsRating>.Failure(PurchaseErrors.PurchaseNotFound);

        // Act
        var result = await _sut.RateInstructor(instructorId, studentId, Rating.CreateRating(5), DateTime.UtcNow, token);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(exprected.Error);
    }


    [Fact]
    public async Task RateInstructor_ShouldReturnInstructorNotTeachingError_WhenPurchasedSessionsdontBelongToTheInstructor()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string instructorId = "1", studentId = "2";
        var session = new Session("1", new Title(""), new Description(""), Money.Zero(), SessionStatus.Publish, DateTime.UtcNow, "10", "2");
        _instructorRatingRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<InstructorsRating>>(), token)
            .ReturnsNull();
        _purchaseRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
            .Returns(new List<Purchase>());
        _sessionRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Session>>(), token)
                .Returns(new List<Session>()
                {
                    session
                });

        var exprected = Result<InstructorsRating>.Failure(RatingsErrors.InstructorNotTeaching);

        // Act
        var result = await _sut.RateInstructor(instructorId, studentId, Rating.CreateRating(5), DateTime.UtcNow, token);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(exprected.Error);
    }

    [Fact]
    public async Task RateInstructor_ShouldReturnSuccessWithInstructorRatingInstance_WhenRatingDoneSuccessfully()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string instructorId = "1", studentId = "2";
        var utc = DateTime.UtcNow;
        var session = new Session("1", new Title(""), new Description(""), Money.Zero(), SessionStatus.Publish, utc, "1", "2");
        _instructorRatingRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<InstructorsRating>>(), token)
            .ReturnsNull();
        _purchaseRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
            .Returns(new List<Purchase>());
        _sessionRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Session>>(), token)
                .Returns(new List<Session>()
                {
                    session
                });

        var instructorRating = InstructorsRating.CreateNewRating(instructorId, studentId, Rating.CreateRating(5), utc);
        var exprected = Result<InstructorsRating>.Succuss(instructorRating);

        // Act
        var result = await _sut.RateInstructor(instructorId, studentId, Rating.CreateRating(5), DateTime.UtcNow, token);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(exprected.Error);
    }

    [Fact]
    public async Task RateInstructor_ShouldRaiseInstructorRatedDomainEvent_WhenRatingDoneSuccessfully()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        string instructorId = "1", studentId = "2";
        var rating = Rating.CreateRating(5);
        var utc = DateTime.UtcNow;
        var session = new Session("1", new Title(""), new Description(""), Money.Zero(), SessionStatus.Publish, utc, "1", "2");
        _instructorRatingRepo.GetWithSpecAsync(Arg.Any<BaseSpecifications<InstructorsRating>>(), token)
            .ReturnsNull();
        _purchaseRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Purchase>>(), token)
            .Returns(new List<Purchase>());
        _sessionRepo.GetAllWithSpecAsync(Arg.Any<BaseSpecifications<Session>>(), token)
                .Returns(new List<Session>()
                {
                    session
                });

        // Act
        var result = await _sut.RateInstructor(instructorId, studentId, rating, DateTime.UtcNow, token);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Value.DomainEvents.Should().HaveCount(1);
        result.Value.DomainEvents[0].Should().Be(new InstructorRatedDomainEvent(instructorId, rating));
    }
}
