using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Ratings;
using ELearning.Domain.Ratings.Events;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Ratings;
public sealed class SessionRatingTests
{
    private readonly SessionsRating _sut;

    public SessionRatingTests()
    {
        _sut = SessionsRating.CreateNewRating("1", "2", Rating.CreateRating(4), DateTime.UtcNow);
    }

    [Fact]
    public void CreateNewRating_ShouldCreateNewRatingToSessionFromStudent_WhenInputsAreValid()
    {
        // Arrange
        var date = DateTime.UtcNow;
        // Act
        var result = SessionsRating.CreateNewRating("1", "2", Rating.CreateRating(4), date);

        // Assert
        result.SessionId.Should().Be("1");
        result.StudentId.Should().Be("2");
        result.Rating.Should().Be(Rating.CreateRating(4));
        result.CreatedAtUtc.Should().Be(date);
    }

    [Theory]
    [InlineData(null!, "2")]
    [InlineData("2", null!)]
    [InlineData("", null!)]
    [InlineData("4", "")]
    public void CreateNewRating_ShouldThrowException_WhenInputsAreInvalid(string? sessionId, string? studentId)
    {
        // Arrange
        var date = DateTime.UtcNow;
        // Act
        Action result = () => SessionsRating.CreateNewRating(sessionId!, studentId!, Rating.CreateRating(4), date);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Invalid input. Input can't be null");
    }

    [Fact]
    public void EnsureCanBeMutated_ShouldReturnFalse_WhenStudentIdDoesntMatch()
    {
        // Arrange

        // Act
        var result = _sut.EnsureCanBeMutated("33");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EnsureCanBeMutated_ShouldReturnTrue_WhenStudentIdMatch()
    {
        // Arrange

        // Act
        var result = _sut.EnsureCanBeMutated("2");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void UpdateRating_ShouldThrowException_WhenRatingIsNull()
    {
        // Arrange

        // Act
        Action result = () => _sut.UpdateRating(null!);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Rating can't by null");
    }

    [Fact]
    public void UpdateRating_ShouldNotDoAnyUpdate_WhenTheNewRatingIsLikeTheOldOne()
    {
        // Arrange
        var newRating = Rating.CreateRating(4);

        // Act
        _sut.UpdateRating(newRating);

        // Assert
        _sut.DomainEvents.Should().HaveCount(0);
        _sut.Rating.Should().Be(newRating);
    }

    [Fact]
    public void UpdateRating_ShouldUpdateRating_WhenTheNewRatingIsValidAndNotMatchTheExistingOne()
    {
        // Arrange
        var newRating = Rating.CreateRating(5);

        // Act
        _sut.UpdateRating(newRating);

        // Assert
        _sut.Rating.Should().Be(newRating);
    }

    [Fact]
    public void UpdateRating_ShouldRaiseRatingUpdatedDomainEvent_WhenTheRatingIsUpdatedSuccessfully()
    {
        // Arrange
        var newRating = Rating.CreateRating(5);
        var oldRating = _sut.Rating;
        // Act
        _sut.UpdateRating(newRating);

        // Assert
        _sut.DomainEvents.Should().HaveCount(1);
        _sut.DomainEvents[0].Should().Be(new SessionRatingUpdatedDomainEvent(_sut.SessionId, oldRating, newRating));
    }
}
