using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using ELearning.Domain.Ratings;
using ELearning.Domain.Sessions.Events;
using ELearning.Domain.Shared;

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
        string instructorId,
        string subjectId,
        string classId) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        Status = status;
        CreatedOnUtc = createdOnUtc;
        InstructorId = instructorId;
        SubjectId = subjectId;
        ClassId = classId;
    }

    public Title Title { get; private set; }
    public Description? Description { get; private set; }
    public Money Price { get; private set; }
    public string InstructorId { get; private set; }
    public string SubjectId { get; private set; }
    public string ClassId { get; private set; }
    public SessionStatus Status { get; private set; }
    public RatingSummary Rating { get; private set; } = new RatingSummary(0, 0);
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public DateTime? LastUpdatedOnUtc { get; private set; }
    public bool HasQuiz { get; private set; }
    public SessionQuiz? Quiz { get; private set; }

    public void AddVideo(
        string id, 
        Title title, 
        string url, 
        VideoOrder order, 
        DateTime utcNow, 
        int durationInSeconds, 
        Percentage thresholdPercentage, 
        bool isInProgression, 
        bool hasPrerequisite,
        string? prerequisiteId = null, 
        int maxViewCount = 3)
    {
        var video = new Video(id, title, url, order, durationInSeconds, thresholdPercentage, isInProgression, hasPrerequisite, prerequisiteId, maxViewCount);
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

    public void Unpublish()
    {
        if (Status != SessionStatus.Publish)
        {
            throw new ApplicationException("Session is already unpublished");
        }

        Status = SessionStatus.Draft;
        PublishedOnUtc = default;
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

    public void AddRating(Rating rating)
    {
        Rating = Rating.Add(rating);
    }

    public void RemoveRating(Rating studentRating)
    {
        if (Rating.Count == 0)
        {
            throw new ApplicationException("Invalid operation. Can't remove rating not exist");
        }

        var newCount = Rating.Count - 1;
        var newTotal = (int)(Rating.Average.Value * Rating.Count - studentRating.Value);

        Rating = newCount == 0 ? new RatingSummary(0, 0) : new RatingSummary(newCount, newTotal);
    }

    public void UpdateRating(Rating oldRating, Rating newRating)
    {
        if (Rating.Count == 0)
        {
            throw new ApplicationException("Invalid operation. Can't update rating not exist");
        }

        if (oldRating.Value == newRating.Value)
        {
            throw new ApplicationException("Error the new value is equal to the old one");
        }

        var newTotal = (int)(Rating.Average.Value * Rating.Count - oldRating.Value + newRating.Value);

        Rating = new RatingSummary(Rating.Count, newTotal);
    }

    public SessionQuiz CreateMcqQuiz(
        string id,
        string instructorId,
        string subjectId,
        Title title,
        Percentage passingPercentage,
        IEnumerable<ExamQuestion> questions, 
        int totalGrades, 
        int questionsNumber,
        int durationInSeconds,
        DateTime createdAt,
        int maximumTries = 3)
    {
        if (Status != SessionStatus.Draft)
        {
            throw new ApplicationException("Can't create quiz for published session");
        }
        if (HasQuiz)
        {
            throw new ApplicationException("You can't create another quiz for same session");
        }

        var isAllMcq = questions.All(q => q.Type == ExamQuestionType.Mcq);

        if (!isAllMcq)
        {
            throw new ApplicationException("Invalid question type. Quiz contains only MCQ questions");
        }

        var quiz = SessionQuiz.Create(id, 
            instructorId, 
            subjectId, 
            title,
            durationInSeconds, 
            totalGrades, 
            questionsNumber, passingPercentage, maximumTries, createdAt, questions);

        HasQuiz = true;
        Quiz = quiz;
        return quiz;
    }

    public void RemoveQuiz(DateTime utcNow)
    {
        if (!HasQuiz)
        {
            throw new ApplicationException("There's no quiz to be removed");
        }

        if (Status != SessionStatus.Draft || PublishedOnUtc.HasValue)
        {
            throw new ApplicationException("Can't remove quiz of published session");
        }

        HasQuiz = false;
        Quiz = null;
        LastUpdatedOnUtc = utcNow;
    }

    public void HideQuizFromSession()
    {
        if (Quiz is null)
        {
            throw new ApplicationException("There's no quiz hide");
        }

        Quiz.HideQuiz();
    }

    public void PublishQuizToSession()
    {
        if (Quiz is null)
        {
            throw new ApplicationException("There's no quiz to publish");
        }

        Quiz.PublishQuiz();
    }
}
