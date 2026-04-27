using ELearning.Api.DTOs.Users;
using ELearning.Application.Assistants.CreateAssistant;
using ELearning.Domain.Users;

namespace ELearning.Api.Mappings.Assistants;

public static class AssistantMappings
{
    public static CreateAssistantCommand ToCreateAssistantCommand(this KeycloakUserDto dto, string id)
    {
        var firstName = new FirstName(dto.FirstName);
        var lastName = new LastName(dto.LastName);
        var email = new Email(dto.Email);

        return new CreateAssistantCommand(firstName, lastName, email, dto.BirthDate, dto.City, id);
    }
}
