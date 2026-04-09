using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Exams;
public interface IExamQuestionRepository
{
    Task<ExamQuestion> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ExamQuestion> GetWithSpecAsync(IBaseSpecifications<ExamQuestion> spec, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExamQuestion>> GetAllWithSpecAsync(IBaseSpecifications<ExamQuestion> spec, CancellationToken cancellationToken = default);
    void Add(ExamQuestion questions);
}
