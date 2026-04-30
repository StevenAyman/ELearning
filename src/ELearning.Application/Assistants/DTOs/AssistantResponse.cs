using ELearning.Api.DTOs.Shared;

namespace ELearning.Application.Assistants.DTOs;

public sealed record AssistantResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public InstructorWithoutRatingDto Instructor { get; init; }
    public LinkDto[]? Links { get; set; }
}
