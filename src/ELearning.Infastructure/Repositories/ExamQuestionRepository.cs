using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;

namespace ELearning.Infastructure.Repositories;
public sealed class ExamQuestionRepository : Repository<ExamQuestion>, IExamQuestionRepository
{
    public ExamQuestionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
