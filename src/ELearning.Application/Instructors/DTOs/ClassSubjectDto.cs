using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Instructors.DTOs;
public sealed record ClassSubjectDto
{
    public required string ClassId { get; init; }
    public required string SubjectId { get; init; }
}
