using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;

namespace ELearning.Infastructure.Repositories;
public sealed class PurchaseMethodRepository : Repository<PurchaseMethod>, IPurchaseMethodRepository
{
    public PurchaseMethodRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
