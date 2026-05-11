using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.PaidCodes.DTOs;
using ELearning.Domain.Purchases;

namespace ELearning.Application.Abstractions.Data;
public interface IPaidCodeReadService
{
    Task<IEnumerable<FullPaidCodeResponse>> GetAllAsync(PaidCodeStatus? status, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    Task<FullPaidCodeResponse?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
