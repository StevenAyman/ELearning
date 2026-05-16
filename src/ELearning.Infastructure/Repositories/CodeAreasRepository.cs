using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;

namespace ELearning.Infastructure.Repositories;
public sealed class CodeAreasRepository : Repository<CodeAreas>, ICodeAreasRepository
{
    public CodeAreasRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public void AddRange(List<CodeAreas> codeAreas)
    {
        _dbContext.Set<CodeAreas>().AddRange(codeAreas);
    }

    public void RemoveRange(List<CodeAreas> codeAreas)
    {
        _dbContext.Set<CodeAreas>().RemoveRange(codeAreas);
    }
}
