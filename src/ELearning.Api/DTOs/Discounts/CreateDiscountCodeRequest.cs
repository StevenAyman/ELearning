using ELearning.Domain.Discounts;

namespace ELearning.Api.DTOs.Discounts;

public sealed record CreateDiscountCodeRequest
{
    public required string Code { get; init; }
    public required DiscountAmountType DiscountType { get; init; }
    public required DiscountExpirationType ExpireType { get; init; }
    public required double Amount { get; init; }
    public int? CountLimit { get; init; }
    public DateTime? ExpirePeriodStart { get; init; }
    public DateTime? ExpirePeriodEnd { get; init; }
    public required int DiscountApplicableAreaId { get; init; }
    public string[] TargetIds { get; init; }

}
