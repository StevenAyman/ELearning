using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Sessions;
public interface ISessionRepository
{
    Task<Session> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IQueryable<Session>> GetAllByInstructorId(string instructorId, CancellationToken cancellationToken = default);
    void Update(Session session);
    void Delete(Session session);

}
