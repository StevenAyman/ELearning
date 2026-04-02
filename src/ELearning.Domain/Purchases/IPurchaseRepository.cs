using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Purchases;
public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAllPurchasesAsync(CancellationToken cancellationToken = default);
    Task<Purchase> GetByIdAsync(string purchaseId, CancellationToken cancellationToken = default);
    void Add(Purchase purchase);
    void Update(Purchase purchase);
    void Delete(string id);
}
