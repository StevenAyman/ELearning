namespace ELearning.Domain.Discounts.DiscountCodeBuilder;

public interface IDiscountExpirationType
{
    IDiscountExpiration WithExpirationType(DiscountExpirationType discountExpirationType);
}
