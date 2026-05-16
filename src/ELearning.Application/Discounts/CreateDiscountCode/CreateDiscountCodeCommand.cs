using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Discounts;

namespace ELearning.Application.Discounts.CreateDiscountCode;
public sealed record CreateDiscountCodeCommand(
    string Code,
    DiscountAmountType DiscountType,
    DiscountExpirationType ExpireType,
    double Amount,
    int? CountLimit,
    DateTime? ExpirePeriodStart,
    DateTime? ExpirePeriodEnd,
    int DiscountAreaId,
    string[] TargetIds) : ICommand<DiscountCodeResponseWithTargets>;
