using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Instructors;

public sealed class InstructorTests
{
    private readonly Instructor _sut;
    public InstructorTests()
    {
        _sut = new Instructor(Guid.NewGuid().ToString(),
            new Bio("This is a valid bio for instructor"),
            new Rating(3),
            $"s_{Guid.CreateVersion7()}");
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
}
