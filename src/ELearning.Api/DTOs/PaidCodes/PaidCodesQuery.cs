using ELearning.Domain.Purchases;

namespace ELearning.Api.DTOs.PaidCodes;

public sealed record PaidCodesQuery
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public PaidCodeStatus? Status { get; init; }
}
