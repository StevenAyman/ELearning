using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Api.DTOs.Shared;

namespace ELearning.Application.Subjects.GetSubject;
public sealed class SubjectDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public LinkDto[]? Links { get; set; }
}
