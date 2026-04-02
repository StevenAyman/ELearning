using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Sessions.Events;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Sessions;
public sealed class SessionTests
{
    private readonly Session _sut;

    public SessionTests()
    {
        _sut = new Session($"s_{Guid.CreateVersion7()}",
            new Title("Session 01"),
            new Description("Session 01 Description"),
            new Money(50),
            SessionStatus.Draft,
            DateTime.UtcNow,
            $"i_{Guid.CreateVersion7()}",
            $"s_{Guid.CreateVersion7()}");
    }

    [Fact]
    public void AddVideo_ShouldAddNewVideo_WhenVideoInputsAreValid()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var id = $"v_{Guid.CreateVersion7()}";
        var video = new Video(id,
            new Title("Video 01"), 
            "url", 
            VideoOrder.Create(1));

        // Act
        _sut.AddVideo(id, new Title("Video 01"), "url", VideoOrder.Create(1), utcNow);

        // Assert
        _sut.Videos.Should().HaveCount(1);
        _sut.Videos.Should().ContainEquivalentOf(video, options => options.Excluding(os => os.DomainEvents));
        _sut.LastUpdatedOnUtc.Should().Be(utcNow);
    }

    [Fact]
    public void AddVideo_ShouldRaiseVideoAddedDomainEvent_WhenNewVideoIsAdded()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var id = $"v_{Guid.CreateVersion7()}";
        var video = new Video(id,
            new Title("Video 01"),
            "url",
            VideoOrder.Create(1));

        // Act
        _sut.AddVideo(id, new Title("Video 01"), "url", VideoOrder.Create(1), utcNow);

        // Assert
        _sut.Videos.Should().ContainEquivalentOf(video, options => options.Excluding(os => os.DomainEvents));
        _sut.Videos.FirstOrDefault(v => v.Id == id).Should().NotBeNull();
        _sut.Videos.FirstOrDefault(v => v.Id == id)?.DomainEvents.Should().HaveCount(1);
        _sut.Videos.FirstOrDefault(v => v.Id == id)?.DomainEvents.Should().Contain(new VideoAddedDomainEvent(id));
    }

    [Fact]
    public void RemoveVideo_ShouldRemoveVideoFromSession_WhenVideoIsExist()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var id = $"v_{Guid.CreateVersion7()}";
        _sut.AddVideo(id, new Title("Video 01"), "url", VideoOrder.Create(1), utcNow);

        // Act
        Action result = () => _sut.RemoveVideo(id);

        // Assert
        _sut.Videos.Should().HaveCount(1);
        result.Invoke();
        _sut.Videos.Should().HaveCount(0);
    }

    [Fact]
    public void RemoveVideo_ShouldThrowException_WhenVideoIsNotFound()
    {
        // Arrange

        // Act
        Action result = () => _sut.RemoveVideo($"v_{Guid.CreateVersion7()}");

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Video is not found");
    }

    [Fact]
    public void UpdatePrice_ShouldUpdatePrice_WhenPriceIsValid()
    {
        // Arrange
        var UpdatedPrice = new Money(80);
        var utcNow = DateTime.UtcNow;

        // Act
        _sut.UpdatePrice(UpdatedPrice, utcNow);

        // Assert
        _sut.Price.Should().Be(UpdatedPrice);
        _sut.LastUpdatedOnUtc.Should().Be(utcNow);
    }

    [Fact]
    public void UpdatePrice_ShouldThrowException_WhenPriceIsNotValid()
    {
        // Arrange
        var UpdatedPrice = Money.Zero();
        var utcNow = DateTime.UtcNow;

        // Act
        Action result = () => _sut.UpdatePrice(UpdatedPrice, utcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Price should be more than 0");
    }

    [Theory]
    [InlineData("Session 02", "Session 01 Description")]
    [InlineData("Session 01", "Session 01 updated description")]
    [InlineData("Session 02", "Session 02 updated description")]
    public void UpdateDetails_ShouldUpdateTitleAndDescription_WhenTheyAreValid(string title, string description)
    {
        // Arrange
        var updatedTitle = new Title(title);
        var updatedDesc = new Description(description);
        var utcNow = DateTime.UtcNow;

        // Act
        _sut.UpdateDetails(updatedTitle, updatedDesc, utcNow);

        // Assert
        _sut.Title.Should().Be(updatedTitle);
        _sut.Description.Should().Be(updatedDesc);
        _sut.LastUpdatedOnUtc.Should().Be(utcNow);
    }

    [Theory]
    [InlineData("", "Description")]
    [InlineData("Session 02", "")]
    [InlineData("", "")]
    public void UpdateDetails_ShouldThrowException_WhenTitleOrDescriptionIsNotValid(string title, string description)
    {
        // Arrange

        // Act
        Action result = () => 
            _sut.UpdateDetails(new Title(title), new Description(description), DateTime.UtcNow);

        // Assert
        result.Should().Throw<ApplicationException>();
    }

    [Fact]
    public void Publish_ShouldChangeTheStateOfSessionToPublished_WhenSessionContainsVideos()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var id = $"v_{Guid.CreateVersion7()}";
        _sut.AddVideo(id, new Title("Video 01"), "url", VideoOrder.Create(1), utcNow);

        // Act
        _sut.Publish(utcNow);

        // Assert
        _sut.Status.Should().Be(SessionStatus.Publish);
        _sut.PublishedOnUtc.Should().Be(utcNow);
    }

    [Fact]
    public void Publish_ShouldRaiseSessionPublishedDomainEvent_WhenSessionIsPublished()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var id = $"v_{Guid.CreateVersion7()}";
        _sut.AddVideo(id, new Title("Video 01"), "url", VideoOrder.Create(1), utcNow);

        // Act
        _sut.Publish(utcNow);

        // Assert
        _sut.DomainEvents.Should().HaveCount(1);
        _sut.DomainEvents.Should().Contain(new SessionPublishedDomainEvent(_sut.Id));
    }

    [Fact]
    public void Publish_ShouldThrowException_WhenSessionDoesntContainAnyVideo()
    {
        // Arrange

        // Act
        Action result = () => _sut.Publish(DateTime.UtcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The session doesn't contain any videos");
    }

    [Fact]
    public void Unpublish_ShouldChangeTheStateOfSessionToDraft_WhenSessionIsPublished()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var id = $"v_{Guid.CreateVersion7()}";
        _sut.AddVideo(id, new Title("Video 01"), "url", VideoOrder.Create(1), utcNow);
        _sut.Publish(utcNow);

        // Act
        _sut.Unpublish();

        // Assert
        _sut.Status.Should().Be(SessionStatus.Draft);
        _sut.PublishedOnUtc.Should().Be(null);
    }

    [Fact]
    public void Unpublish_ShouldThrowException_WhenSessionIsInDraftState()
    {
        // Arrange

        // Act
        Action result = () => _sut.Unpublish();

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Session is already unpublished");
    }
}
