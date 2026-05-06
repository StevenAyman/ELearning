namespace ELearning.Application.Instructors.DTOs;

public class InstructorSubjectsDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public IEnumerable<ClassSubjectToReturnDto> ClassSubjects { get; set; }
}
