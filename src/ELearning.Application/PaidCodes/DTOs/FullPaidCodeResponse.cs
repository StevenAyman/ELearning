using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;

namespace ELearning.Application.PaidCodes.DTOs;
public sealed record FullPaidCodeResponse
{
    public string Id { get; private set; }
    public string Code { get; private set; }
    public decimal Balance { get; private set; }
    public string Status { get; private set; }
    public string? StudentId { get; private set; }
    public DateTime GeneratedAtUtc { get; private set; }
    public DateTime? UsedAtUtc { get; private set; }
    public DateTime? ExpiredAtUtc { get; private set; }
}
