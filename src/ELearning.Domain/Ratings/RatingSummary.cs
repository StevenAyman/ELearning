using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Ratings;

public sealed record RatingSummary
{
    private readonly int _total;
    private RatingSummary() { }
    public RatingSummary(int count, int total)
    {
        _total = total;
        Count = count;
        Average = count == 0? Rating.CreateRating(0) : Rating.CreateRating((decimal)total / count);
    }

    public int Count { get; init; }
    public Rating Average { get; init; }

    public RatingSummary Add(Rating rating)
    {

        var newTotal = _total + (int)rating.Value;
        var count = Count + 1;

        return new RatingSummary(count, newTotal);
    }

}
