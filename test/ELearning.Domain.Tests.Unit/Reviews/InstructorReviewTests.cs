using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Reviews;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Reviews;
public sealed class InstructorReviewTests
{
    private readonly InstructorsReview _sut;

    public InstructorReviewTests()
    {
        _sut = new InstructorsReview("1", "2", new Review("Review That"));
    }

    [Fact]
    public void EnsureCanBeMutated_ShouldReturnFalse_WhenGivenStudentIdNotMatch()
    {
        // Arrange

        // Act
        var result = _sut.EnsureCanBeMutated("1");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EnsureCanBeMutated_ShouldReturnTrue_WhenGivenStudentIdMatches()
    {
        // Arrange

        // Act
        var result = _sut.EnsureCanBeMutated(_sut.StudentId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void UpdateReview_ShouldThrowException_WhenReviewIsInvalid()
    {
        // Arrange


        // Act
        Action result = () => _sut.UpdateReview(null!);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Sorry review is invalid");
    }

    [Fact]
    public void UpdateReview_ShouldUpdateReview_WhenReviewIsValid()
    {
        // Arrange
        var newReview = new Review("Updated Review");

        // Act
        _sut.UpdateReview(newReview);

        // Assert
        _sut.Review.Should().Be(newReview);
    }
}
