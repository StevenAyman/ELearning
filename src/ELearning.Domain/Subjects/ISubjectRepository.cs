using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Subjects;
public interface ISubjectRepository
{
    Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Subject?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Subject?> GetWithSpecAsync(IBaseSpecifications<Subject> spec, CancellationToken cancellationToken = default);
    void Update(Subject subject);
    void Delete(Subject subject);
    void Add(Subject subject);
}
