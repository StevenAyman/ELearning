using ELearning.Domain.Discounts;

namespace ELearning.Api.DTOs.Discounts;

public sealed record DiscountCodeQuery
{
    public DiscountStatus? Status { get; init; }
    public DiscountAmountType? DiscountType { get; init; }
    public DiscountExpirationType? ExpireType { get; init; }
    public string? Code { get; init; }
}
