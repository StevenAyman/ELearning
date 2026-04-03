using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Ratings;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Instructors;
public sealed class Instructor : BaseEntity
{
    private Instructor() { }
    public Instructor(
        string id,
        Bio bio,
        string subjectId) : base(id)
    {
        Bio = bio;
        SubjectId = subjectId;
    }
    public Bio Bio { get; private set; }
    public RatingSummary Rating { get; private set; } = new RatingSummary(0, 0);
    public string SubjectId { get; private set; }

    public void UpdateBio(Bio bio)
    {
        if (!string.IsNullOrWhiteSpace(bio.Value))
        {
            Bio = bio;
            return;
        }

        throw new ApplicationException("Bio can't be null or empty");
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

        Rating =  new RatingSummary(Rating.Count, newTotal);
    }

}
