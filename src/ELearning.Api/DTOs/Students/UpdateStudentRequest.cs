namespace ELearning.Api.DTOs.Students;

public sealed record UpdateStudentRequest
{
    public string ClassId { get; init; }
}

