using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Api.DTOs.Shared;
using ELearning.Application.Sessions.DTOs;
using ELearning.Domain.Instructors;

namespace ELearning.Application.Instructors.DTOs;
public sealed record InstructorWithSessionsDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Bio { get; init; }
    public decimal Rating { get; init; }
    public int RatingCount { get; init; }
    public IReadOnlyList<SessionDto> Sessions { get; set; }
    public LinkDto[]? Links { get; set; }
}
