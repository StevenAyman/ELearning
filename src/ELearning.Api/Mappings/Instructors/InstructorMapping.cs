using ELearning.Api.DTOs.Instructors;
using ELearning.Application.Instructors.CreateInstructor;
using ELearning.Domain.Users;

namespace ELearning.Api.Mappings.Instructors;

public static class InstructorMapping
{
    public static CreateInstructorCommand ToCreateInstructorCommand(this CreateInstructorRequest dto, string id)
    {
        var firstName = new FirstName(dto.FirstName);
        var lastName = new LastName(dto.LastName);
        var email = new Email(dto.Email);

        return new CreateInstructorCommand(firstName, lastName, email, dto.BirthDate, dto.City, id, dto.SubjectId);
    }
}
