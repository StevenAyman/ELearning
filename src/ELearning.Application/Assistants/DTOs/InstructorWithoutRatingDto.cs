namespace ELearning.Application.Assistants.DTOs;

public sealed record InstructorWithoutRatingDto
{
    public string InstructorId { get; init; }
    public string Name { get; init; }
}
