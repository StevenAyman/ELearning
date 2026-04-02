namespace ELearning.Domain.Discounts.DiscountCodeBuilder;

public interface ICode
{
    IDiscountAmountType WithCode(string code);
}
