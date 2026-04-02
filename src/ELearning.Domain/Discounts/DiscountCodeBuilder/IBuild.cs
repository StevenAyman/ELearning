namespace ELearning.Domain.Discounts.DiscountCodeBuilder;

public interface IBuild
{
    DiscountCode Build(DateTime utcNow);
}
