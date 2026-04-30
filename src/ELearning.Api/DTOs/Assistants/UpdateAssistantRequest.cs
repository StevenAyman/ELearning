namespace ELearning.Api.DTOs.Assistants;

public sealed record UpdateAssistantRequest
{
    public string InstructorId { get; init; }
}
