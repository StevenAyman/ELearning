using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Discounts.DTOs;
public sealed record DiscountCodeResponseWithTargets
{
    public string Id { get; init; }
    public string Code { get; init; }
    public string DiscountType { get; init; }
    public string ExpireType { get; init; }
    public decimal DiscountAmount { get; init; }
    public int? CountLimit { get; init; }
    public int UsageNumber { get; init; }
    public string Status { get; init; }
    public ApplicableAreaDto ApplicableArea { get; set; }
    public HashSet<AreaTargetDto> AreaTargets { get; init; } = new HashSet<AreaTargetDto>();
    public DateTime? ExpirePeriodStart { get; init; }
    public DateTime? ExpirePeriodEnd { get; init; }
    public DateTime? ExpiredAtUtc { get; init; }
    public DateTime? LastUsedAtUtc { get; init; }
}


public sealed record ApplicableAreaDto
{
    public int Id { get; init; }
    public string Area { get; init; }
}

public sealed record AreaTargetDto
{
    public string Id { get; init; }
    public string Name { get; init; }
}

public sealed record DiscountCodeResponse
{
    public string Id { get; init; }
    public string Code { get; init; }
    public string DiscountType { get; init; }
    public string ExpireType { get; init; }
    public decimal DiscountAmount { get; init; }
    public int? CountLimit { get; init; }
    public int UsageNumber { get; init; }
    public string Status { get; init; }
    public DateTime? ExpirePeriodStart { get; init; }
    public DateTime? ExpirePeriodEnd { get; init; }
    public DateTime? ExpiredAtUtc { get; init; }
    public DateTime? LastUsedAtUtc { get; init; }
}
