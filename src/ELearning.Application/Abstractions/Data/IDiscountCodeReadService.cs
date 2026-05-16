using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Discounts;

namespace ELearning.Application.Abstractions.Data;
public interface IDiscountCodeReadService
{
    Task<DiscountCodeResponseWithTargets?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DiscountCodeResponse>> GetAllAsync(
        string? code,
        DiscountAmountType? discountType,
        DiscountExpirationType? expireType,
        DiscountStatus? status, 
        CancellationToken cancellationToken = default);
}
