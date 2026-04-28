namespace ELearning.Api.DTOs.Subjects;

public sealed record CreateSubjectDto
{
    public required string SubjectName { get; init; }
}
