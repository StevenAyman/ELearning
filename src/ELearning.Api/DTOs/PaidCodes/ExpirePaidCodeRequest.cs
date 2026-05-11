namespace ELearning.Api.DTOs.PaidCodes;

public sealed record ExpirePaidCodeRequest
{
    public string Code { get; init; }
}
