namespace ELearning.Application.Instructors.DTOs;

public sealed record ClassSubjectToReturnDto
{
    public string ClassId { get; init; }
    public string Class { get; init; }
    public string SubjectId { get; init; }
    public string Subject { get; init; }
}
