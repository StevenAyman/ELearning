using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;

namespace ELearning.Application.Discounts.UpdateDiscountCode;
public sealed record UpdateDiscountCodeCommand(
    string Id,
    string Code,
    DiscountAmountType DiscountAmountType,
    DiscountExpirationType ExpireType,
    decimal Amount,
    DateTime? StartExpirePeriod,
    DateTime? EndExpirePeriod,
    int? CountLimit,
    int ApplicableAreaId,
    string[] TargetIds) : ICommand;
