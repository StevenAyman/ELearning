using ELearning.Api.Services.Sorting;
using ELearning.Application.Students.DTOs;
using ELearning.Domain.Students;

namespace ELearning.Api.Mappings.Students;

public sealed class StudentMapping
{
    public static readonly SortMappingDefinition<StudentDto, Student> SortMappings = new()
    {
        Mappings = [
            new SortMapping(nameof(StudentDto.FirstName), "first_name"),
            new SortMapping(nameof(StudentDto.LastName), "last_name"),
            new SortMapping(nameof(StudentDto.Email), "email"),
            new SortMapping(nameof(StudentDto.BirthDate), "date_of_birth"),
            new SortMapping(nameof(StudentDto.JoinedOn), "joined_on_utc"),
            new SortMapping(nameof(StudentDto.Wallet), "wallet")
            ]
    };
}
