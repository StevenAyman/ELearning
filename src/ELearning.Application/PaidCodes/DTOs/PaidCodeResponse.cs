using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;

namespace ELearning.Application.PaidCodes.DTOs;
public sealed record PaidCodeResponse
{
    public string Id { get; init; }
    public string Code { get; init; }
    public decimal Amount { get; init; }
    public string Status { get; init; }
}
