using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Discounts.DTOs;

namespace ELearning.Application.Abstractions.Data;
public interface IDiscountAreaReadService
{
    Task<IEnumerable<DiscountAreaResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DiscountAreaResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(string area, CancellationToken cancellationToken = default);
    Task<DiscountAreaResponse?> GetByNameAsync(string area, CancellationToken cancellationToken = default);
}
