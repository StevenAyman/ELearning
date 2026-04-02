using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Subjects;
public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllSubjectsAsync(CancellationToken cancellationToken = default);
    Task<Subject> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
