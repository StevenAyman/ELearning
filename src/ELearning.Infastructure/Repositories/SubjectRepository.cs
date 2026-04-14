using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Subjects;

namespace ELearning.Infastructure.Repositories;
public sealed class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
