namespace ELearning.Api.DTOs.PaidCodes;

public sealed record PaidCodeCreateRequest
{
    public required int Count { get; init; }
    public required decimal Balance { get; init; }
}
