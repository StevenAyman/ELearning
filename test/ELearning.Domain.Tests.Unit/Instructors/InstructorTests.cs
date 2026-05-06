using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Ratings;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Instructors;

public sealed class InstructorTests
{
    private readonly Instructor _sut;
    public InstructorTests()
    {
        _sut = new Instructor(Guid.NewGuid().ToString(),
            new Bio("This is a valid bio for instructor"));
    }

    [Fact]
    public void UpdateBio_ShouldUpdateInstructorBio_WhenBioIsValid()
    {
        // Arrange
        var newBio = new Bio("This is updated valid bio");
       
        // Act
        _sut.UpdateBio(newBio);

        // Assert
        _sut.Bio.Should().Be(newBio);
    }

    [Fact]
    public void UpdateBio_ShouldThrowException_WhenBioIsNotValid()
    {
        // Arrange
        var newBio = new Bio("");

        // Act
        Action result = () => _sut.UpdateBio(newBio);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Bio can't be null or empty");
    }

    [Fact]
    public void AddRating_ShouldAddRatingToInstructor_WhenNewStudentRatingHim()
    {
        // Arrange
        var rating = Rating.CreateRating(5);

        // Act
        _sut.AddRating(rating);

        // Assert
        _sut.Rating.Count.Should().Be(1);
        _sut.Rating.Average.Should().Be(rating);
    }

    [Fact]
    public void RemoveRating_ShouldThrowException_WhenInstructorHasNoRatingsAndStudentTryingToRemoveHisRating()
    {
        // Arrange
        var rating = Rating.CreateRating(5);

        // Act
        Action result = () => _sut.RemoveRating(rating);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Invalid operation. Can't remove rating not exist");
    }

    [Fact]
    public void RemoveRating_ShouldRemoveStudentRatingAndRecalculateInstructorRatingSuccessfully_WhenStudentRemoveHisRatingForInstructor()
    {
        // Arrange
        var rating = Rating.CreateRating(5);
        _sut.AddRating(rating);

        // Act
        _sut.RemoveRating(rating);

        // Assert
        _sut.Rating.Should().BeEquivalentTo(new RatingSummary(0, 0));
    }

    [Fact]
    public void RemoveRating_ShouldRemoveStudentRatingAndRecalculateInstructorRating_WhenStudentRemoveRatingAndInstructorHasOtherRatings()
    {
        // Arrange
        var rating = Rating.CreateRating(5);
        var rating2 = Rating.CreateRating(4);
        _sut.AddRating(rating);
        _sut.AddRating(rating2);

        // Act
        _sut.RemoveRating(rating);

        // Assert
        _sut.Rating.Should().BeEquivalentTo(new RatingSummary(1, 4));
    }

    [Fact]
    public void UpdateRating_ShouldThrowException_WhenInstructorHasNoRatingsAndStudentTryingToUpdateHisRating()
    {
        // Arrange
        var oldRating = Rating.CreateRating(5);
        var rating = Rating.CreateRating(4);

        // Act
        Action result = () => _sut.UpdateRating(oldRating, rating);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Invalid operation. Can't update rating not exist");
    }

    [Fact]
    public void UpdateRating_ShouldThrowException_WhenTheNewRatingIsSameAsTheOldOne()
    {
        // Arrange
        var oldRating = Rating.CreateRating(5);
        var rating = Rating.CreateRating(5);
        _sut.AddRating(oldRating);

        // Act
        Action result = () => _sut.UpdateRating(oldRating, rating);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Error the new value is equal to the old one");
    }

    [Fact]
    public void UpdateRating_ShouldUpdateInstructorRating_WhenStudentUpdateHisRating()
    {
        // Arrange
        var oldRating = Rating.CreateRating(4);
        var rating = Rating.CreateRating(5);
        _sut.AddRating(oldRating);

        // Act
        _sut.UpdateRating(oldRating, rating);

        // Assert
        _sut.Rating.Should().BeEquivalentTo(new RatingSummary(1, 5));
    }
}
