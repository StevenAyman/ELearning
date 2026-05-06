using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Api.DTOs.Shared;
using ELearning.Application.Subjects.GetSubject;

namespace ELearning.Application.Classes.DTOs;
public sealed record ClassWithSubjectsDto
{
    public string Id { get; init; }
    public string Class { get; init; }
    public IEnumerable<SubjectDto> Subjects { get; set; }
    public LinkDto[]? Links { get; set; }
}
