using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Enrollments;
public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Enrollment?> GetWithSpecAsync(IBaseSpecifications<Enrollment> spec, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Enrollment>> GetAllWithSpecAsync(IBaseSpecifications<Enrollment> spec, CancellationToken cancellationToken = default);
    void Add(Enrollment enrollment);
    void Update(Enrollment enrollment);
    void Delete(Enrollment enrollment);

}
