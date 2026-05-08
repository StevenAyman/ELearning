using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.DTOs.Students;

public sealed record GetAllStudentsRequest
{
    private const int SIZE = 5;
    private int? _pageSize;
    [FromQuery(Name = "q")]
    public string? Search { get; set; }
    public int? PageIndex { get; set; } = 1;
    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value is null || value < 5 || value > 20 ? SIZE : value;
    }
    public string? ClassId { get; set; }
    public string? SubjectId { get; set; }
    public string? InstructorId { get; set; }
    public string? Sort { get; set; }
    [FromHeader(Name = "Accept")]
    public string Accept { get; init; }

}
