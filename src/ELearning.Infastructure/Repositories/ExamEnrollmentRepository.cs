using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;

namespace ELearning.Infastructure.Repositories;
public sealed class ExamEnrollmentRepository : Repository<ExamEnrollment>, IExamEnrollmentRepository
{
    public ExamEnrollmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
