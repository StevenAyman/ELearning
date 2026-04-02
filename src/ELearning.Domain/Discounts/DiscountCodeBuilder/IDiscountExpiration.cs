namespace ELearning.Domain.Discounts.DiscountCodeBuilder;

public interface IDiscountExpiration
{
    IBuild WithCountLimit(int countLimit);
    IBuild WithExpirePeriod(DateRange expirePeriod);
}
