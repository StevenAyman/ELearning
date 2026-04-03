using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings.Events;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;

namespace ELearning.Domain.Ratings;
public sealed class InstructorsRating : BaseEntity
{
    private InstructorsRating() { }
    private InstructorsRating(string instructorId, string studentId, Rating rating, DateTime createdAtUtc) : base($"ir_{Guid.CreateVersion7()}")
    {
        InstructorId = instructorId;
        StudentId = studentId;
        Rating = rating;
        CreatedAtUtc = createdAtUtc;
    }

    public string InstructorId { get; private set; }
    public string StudentId { get; private set; }
    public Rating Rating { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static InstructorsRating CreateNewRating(
        string instructorId,
        string studentId,
        Rating rating,
        DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(instructorId) ||
            string.IsNullOrWhiteSpace(studentId) ||
            rating is null)
        {
            throw new ApplicationException($"Invalid input. Input can't be null");
        }

        return new InstructorsRating(instructorId, studentId, rating, createdAtUtc);
    }

    public void UpdateRating(Rating rating)
    {
        if (rating is null)
        {
            throw new ApplicationException("Rating can't by null");
        }

        if (Rating != rating)
        {
            RaiseDomainEvent(new InstructorRatingUpdatedDomainEvent(InstructorId, Rating, rating));
            Rating = rating;
        }
    }

    public bool EnsureCanBeMutated(string studentId)
    {
        if(StudentId != studentId)
        {
            return false;
        }
        return true;
    }
}
