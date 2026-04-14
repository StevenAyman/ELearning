using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Sessions;
public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Session?> GetWithSpecAsync(IBaseSpecifications<Session> specs, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Session>> GetAllWithSpecAsync(IBaseSpecifications<Session> specs, CancellationToken cancellationToken = default);

    void Update(Session session);
    void Delete(Session session);
    void Add(Session session);

}
