using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Sessions.DTOs;
public sealed record SessionDto
{
    public string Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public DateTime CreatedOn { get; init; }
}
