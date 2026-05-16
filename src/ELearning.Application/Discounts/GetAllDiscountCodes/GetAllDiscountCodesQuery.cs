using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Discounts;

namespace ELearning.Application.Discounts.GetAllDiscountCodes;
public sealed record GetAllDiscountCodesQuery(
    DiscountStatus? Status, 
    DiscountAmountType? DiscountType,
    DiscountExpirationType? ExpireType,
    string? Code) : IQuery<IEnumerable<DiscountCodeResponse>>;
