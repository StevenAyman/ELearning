using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;

namespace ELearning.Infastructure.Repositories;
public sealed class PaidCodeRepository : Repository<PaidCode>, IPaidCodeRepository
{
    public PaidCodeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
