using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions.Events;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;

namespace ELearning.Domain.Sessions;
public sealed class Session : BaseEntity
{
    private readonly List<Video> _videos = new();
    public IReadOnlyList<Video> Videos => _videos;
    private Session() { }

    public Session(
        string id,
        Title title,
        Description description,
        Money price,
        SessionStatus status,
        DateTime createdOnUtc,
        string instructorId) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        Status = status;
        CreatedOnUtc = createdOnUtc;
        InstructorId = instructorId;
    }

    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Money Price { get; private set; }
    public string InstructorId { get; private set; }
    public SessionStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime PublishedOnUtc { get; private set; }
    public DateTime LastUpdatedOnUtc { get; private set; }

    public void AddVideo(string id, Title title, string url, VideoOrder order, DateTime utcNow)
    {
        var video = new Video(id, title, url, order);
        _videos.Add(video);
        video.RaiseDomainEvent(new VideoAddedDomainEvent(video.Id));
        LastUpdatedOnUtc = utcNow;
    }

    public void RemoveVideo(string id)
    {
        var video = _videos.Find(v => v.Id == id);
        if (video is null)
        {
            throw new ApplicationException("Video is not found");
        }

        _videos.Remove(video);
    }

    public void Publish(DateTime publishedOnUtc)
    {
        if (!_videos.Any())
        {
            throw new ApplicationException("The session doesn't contain any videos");
        }

        Status = SessionStatus.Publish;
        PublishedOnUtc = publishedOnUtc;
        RaiseDomainEvent(new SessionPublishedDomainEvent(Id));

    }

    public void UpdatePrice(Money price, DateTime utcNow)
    {
        if (price is not null && price != Money.Zero())
        {
            Price = price;
            LastUpdatedOnUtc = utcNow;
            return;
        }

        throw new ApplicationException("Price should be more than 0");
    }

    public void UpdateDetails(Title title, Description description, DateTime utcNow)
    {
        if (title is not null && !string.IsNullOrWhiteSpace(title.Value))
        {
            Title = title; 
        }
        else
        {

            throw new ApplicationException("Title shouldn't be null or empty");
        }

        if (description is not null && !string.IsNullOrWhiteSpace(description.Value))
        {
            Description = description;
        }
        else
        {
            throw new ApplicationException("Description shouldn't be null or empty");
        }

        LastUpdatedOnUtc = utcNow;
    }

}
