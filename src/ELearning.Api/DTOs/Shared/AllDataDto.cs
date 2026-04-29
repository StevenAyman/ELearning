namespace ELearning.Api.DTOs.Shared;

public sealed class AllDataDto<T>
{
    public IEnumerable<T> Data { get; init; }
}
