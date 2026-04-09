using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Exams;
public interface IQuestionRepository
{
    Task<ExamQuestion> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExamQuestion>> GetAllWithSpecAsync(IBaseSpecifications<ExamQuestion> spec, CancellationToken cancellationToken = default);
}
