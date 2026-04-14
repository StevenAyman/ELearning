using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Purchases;
public interface IPurchaseRepository
{
    Task<IReadOnlyList<Purchase>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Purchase?> GetByIdAsync(string purchaseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Purchase>> GetAllWithSpecAsync(IBaseSpecifications<Purchase> specs, CancellationToken cancellationToken = default);
    Task<Purchase?> GetWithSpecAsync(IBaseSpecifications<Purchase> specs, CancellationToken cancellationToken = default);
    void Add(Purchase purchase);
    void Update(Purchase purchase);
    void Delete(Purchase purchase);
}
