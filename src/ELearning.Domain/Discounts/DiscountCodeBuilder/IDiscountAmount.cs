using ELearning.Domain.Shared;

namespace ELearning.Domain.Discounts.DiscountCodeBuilder;

public interface IDiscountAmount
{
    IDiscountExpirationType WithDiscountAmount(Money discountAmount);
}
