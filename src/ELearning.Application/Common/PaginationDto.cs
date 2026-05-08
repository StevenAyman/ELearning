using ELearning.Api.DTOs.Shared;

namespace ELearning.Application.Common;

public class PaginationDto<T>
{
    public int PageIndex { get; init; }  
    public int PageSize { get; init; } 
    public int TotalCount { get; init; }
    public bool HasNext => TotalCount > PageIndex * PageSize;
    public bool HasPrevious => PageIndex > 1;
    public List<T> Data { get; init; }
    public List<LinkDto> Links { get; set; }
}
