using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Exams;
public interface IExamRepository
{
    Task<Exam?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Exam>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Exam?> GetWithSpecAsync(IBaseSpecifications<Exam> spec,  CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Exam>> GetAllWithSpecAsync(IBaseSpecifications<Exam> spec, CancellationToken cancellationToken = default);
    void Add(Exam exam);
    void Update(Exam exam);
    void Delete(Exam exam);
}
