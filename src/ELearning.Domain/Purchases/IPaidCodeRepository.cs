using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Purchases;
public interface IPaidCodeRepository
{
    Task<IReadOnlyList<PaidCode>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaidCode?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<PaidCode?> GetWithSpecAsync(IBaseSpecifications<PaidCode> specs, CancellationToken cancellationToken = default);
    void Add(PaidCode paidCode);
    void Update(PaidCode paidCode);
    void Delete(PaidCode paidCode);
}
