using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Infastructure.Repositories;
public sealed class SessionRepository : Repository<Session>, ISessionRepository
{
    public SessionRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }

    public bool IsIdsExist(string[] ids)
    {
        var uniqueIds = ids.Distinct();
        var count = _dbContext.Set<Session>()
            .Where(s => uniqueIds.Contains(s.Id))
            .Count();

        return count == uniqueIds.Count();
    }
}
