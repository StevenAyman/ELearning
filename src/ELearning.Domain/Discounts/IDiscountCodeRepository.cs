using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Discounts;
public interface IDiscountCodeRepository
{
    Task<DiscountCode?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<DiscountCode?> GetWithSpecAsync(IBaseSpecifications<DiscountCode> spec, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DiscountCode>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DiscountCode>> GetAllWithSpecAsync(IBaseSpecifications<DiscountCode> spec, CancellationToken cancellationToken = default);
    void Add(DiscountCode discountCode);
    void Update(DiscountCode discountCode);
    void Delete(DiscountCode discountCode);
}
