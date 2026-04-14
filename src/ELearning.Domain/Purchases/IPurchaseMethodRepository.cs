using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Purchases;
public interface IPurchaseMethodRepository
{
    Task<PurchaseMethod?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseMethod>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(PurchaseMethod purchaseMethod);
    void Update(PurchaseMethod purchaseMethod);
    void Delete(PurchaseMethod purchaseMethod);
}
