using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Sessions;
public sealed class VideoTests
{
    private readonly Video _sut;

    public VideoTests()
    {
        _sut = new($"v_{Guid.CreateVersion7()}", new Title("Video 01"), "valid url", VideoOrder.Create(1));
    }

    [Fact]
    public void UpdateTitle_ShouldUpdateVideoTitle_WhenItIsAValidTitle()
    {
        // Arrange
        var newTitle = new Title("Updated Valid Title");

        // Act
        _sut.UpdateTitle(newTitle);

        // Assert
        _sut.Title.Should().Be(newTitle);
    }

    [Fact]
    public void UpdateTitle_ShouldThrowAnException_WhenTitleIsNotValid()
    {
        // Arrange
        var newTitle = new Title("");

        // Act
        Action result = () => _sut.UpdateTitle(newTitle);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Video title shouldn't be empty");
    }

    [Fact]
    public void UpdateUrl_ShouldUpdateVideoUrl_WhenGivenUrlIsValid()
    {
        // Arrange
        var url = "Valid updated Url";

        // Act
        _sut.UpdateUrl(url);

        // Assert
        _sut.Url.Should().Be(url);
    }

    [Fact]
    public void UpdateUrl_ShouldThrowException_WhenGivenUrlIsNotValid()
    {
        // Arrange
        var newUrl = "";

        // Act
        Action result = () => _sut.UpdateUrl(newUrl);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Video url shouldn't be empty");
    }

    [Fact]
    public void UpdateOrder_ShouldUpdateOrder_WhenAValidOrderIsGiven()
    {
        // Arrange
        var updatedOrder = VideoOrder.Create(2);

        // Act
        _sut.UpdateOrder(updatedOrder);

        // Assert
        _sut.Order.Should().Be(updatedOrder);
    }
}
