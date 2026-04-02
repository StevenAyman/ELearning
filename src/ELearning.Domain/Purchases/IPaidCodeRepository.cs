using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Purchases;
public interface IPaidCodeRepository
{
    Task<IEnumerable<PaidCode>> GetAllPaidCodeAsync(CancellationToken cancellationToken = default);
    Task<PaidCode> GetById(string id, CancellationToken cancellationToken = default);
    void Add(PaidCode paidCode);
    void Update(PaidCode paidCode);
    void Delete(string id);
}
