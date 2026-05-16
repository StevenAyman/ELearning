using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Subjects;

namespace ELearning.Infastructure.Repositories;
public sealed class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public bool IsIdsExist(string[] ids)
    {
        var uniqueIds = ids.Distinct();
        var count = _dbContext.Set<Subject>()
            .Where(s => uniqueIds.Contains(s.Id))
            .Count();

        return count == uniqueIds.Count();
    }

}
