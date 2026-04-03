using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Ratings.Events;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Ratings;
public sealed class SessionsRating : BaseEntity
{
    private SessionsRating() { }
    private SessionsRating(string sessionId, string studentId, Rating rating, DateTime createdAtUtc) : base($"sr_{Guid.CreateVersion7()}")
    {
        SessionId = sessionId;
        StudentId = studentId;
        Rating = rating;
        CreatedAtUtc = createdAtUtc;
    }
    public string SessionId { get; private set; }
    public string StudentId { get; private set; }
    public Rating Rating { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static SessionsRating CreateNewRating(
    string sessionId,
    string studentId,
    Rating rating,
    DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(sessionId) ||
            string.IsNullOrWhiteSpace(studentId) ||
            rating is null)
        {
            throw new ApplicationException($"Invalid input. Input can't be null");
        }

        return new SessionsRating(sessionId, studentId, rating, createdAtUtc);
    }

    public void UpdateRating(Rating rating)
    {
        if (rating is null)
        {
            throw new ApplicationException("Rating can't by null");
        }

        if (Rating != rating)
        {
            RaiseDomainEvent(new SessionRatingUpdatedDomainEvent(SessionId, Rating, rating));
            Rating = rating;
        }
    }

    public bool EnsureCanBeMutated(string studentId)
    {
        if (StudentId != studentId)
        {
            return false;
        }
        return true;
    }
}
