using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Ratings;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Ratings;
public sealed class RatingSummaryTests
{
    private readonly RatingSummary _sut;

    public RatingSummaryTests()
    {
        _sut = new RatingSummary(1, 4);
    }

    [Fact]
    public void Add_ShouldReturnUpdatedRatingSummary_WhenARatingIsAdded()
    {
        // Arrange
        var newRating = Rating.CreateRating(5);
        var expected = new RatingSummary(2, 5 + 4);

        // Act
        var result = _sut.Add(newRating);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
        result.Count.Should().Be(2);
        result.Average.Should().Be(Rating.CreateRating(4.5m));
    }
}
