namespace ELearning.Domain.Shared;

public sealed record Rating
{
    private Rating(decimal rating)
    {
        Value = rating;
    }
    public decimal Value { get; private set; }

    public static Rating CreateRating(decimal rating)
    {
        if (rating < 0 || rating > 5)
        {
            throw new ApplicationException("Error. Rating is out of range");
        }

        return new Rating(rating);
    }

}
