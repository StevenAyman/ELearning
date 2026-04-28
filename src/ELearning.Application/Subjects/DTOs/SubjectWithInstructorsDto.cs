using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Api.DTOs.Shared;
using ELearning.Application.Instructors.DTOs;

namespace ELearning.Application.Subjects.DTOs;
public sealed class SubjectWithInstructorsDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public IReadOnlyList<InstructorDto> Instructors { get; init; }
    public LinkDto[]? Links { get; set; }
}
