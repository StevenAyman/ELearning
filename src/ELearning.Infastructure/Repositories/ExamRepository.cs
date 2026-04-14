using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;

namespace ELearning.Infastructure.Repositories;
public sealed class ExamRepository : Repository<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
