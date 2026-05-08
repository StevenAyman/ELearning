using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Api.DTOs.Shared;

namespace ELearning.Application.Students.DTOs;
public sealed record StudentDto
{
    public string Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime BirthDate { get; init; }
    public string Email { get; init; }
    public decimal Wallet { get; init; }
    public string Class { get; init; }
    public string JoinedOn { get; init; }
    public LinkDto[] Links { get; set; }
}
