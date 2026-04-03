using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Ratings;
public interface ISessionsRating
{
    Task<bool> ExistsAsync(string sessionId, string studentId, CancellationToken cancellationToken = default);
    Task<SessionsRating> GetWithSpecAsync(IBaseSpecifications<SessionsRating> specs, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SessionsRating>> GetAllWithSpecAsync(IBaseSpecifications<SessionsRating> specs, CancellationToken cancellationToken = default);
    void Add(SessionsRating sessionsRatings);
    void Update(SessionsRating sessionsRatings);
    void Delete(SessionsRating sessionsRatings);
}
