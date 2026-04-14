using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Exams;
public interface IExamQuestionAnswerRepository
{
    Task<ExamQuestionAnswer?> GetWithSpecAsync(IBaseSpecifications<ExamQuestionAnswer> spec, CancellationToken cancellationToken = default);
    void Add(ExamQuestionAnswer questionAnswer);
    void Update(ExamQuestionAnswer questionAnswer);
    void Delete(ExamQuestionAnswer questionAnswer);

}
