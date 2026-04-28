namespace ELearning.Api.DTOs.Shared;

public sealed class AllDataDto<T>
{
    public IReadOnlyList<T> Data { get; init; }
}
