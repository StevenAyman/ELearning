using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;

namespace ELearning.Infastructure.Repositories;
public sealed class DiscountCodeRepository : Repository<DiscountCode>, IDiscountCodeRepository
{
    public DiscountCodeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
