using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Exams;
public interface IExamEnrollmentRepository
{
    Task<ExamEnrollment?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ExamEnrollment?> GetWithSpecAsync(IBaseSpecifications<ExamEnrollment> spec,  CancellationToken cancellationToken = default);
    void Add(ExamEnrollment examEnrollment);
    void Update(ExamEnrollment examEnrollment);
    void Delete(ExamEnrollment examEnrollment);
}
