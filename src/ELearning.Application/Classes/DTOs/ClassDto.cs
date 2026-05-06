using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Api.DTOs.Shared;

namespace ELearning.Application.Classes.DTOs;
public sealed class ClassDto
{
    public string Id { get; init; }
    public string Class { get; init; }
    public LinkDto[]? Links { get; set; }
}
