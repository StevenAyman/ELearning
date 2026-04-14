using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Ratings;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Infastructure.Repositories;
public sealed class SessionsRatingRepository : Repository<SessionsRating>, ISessionsRatingRepository
{
    public SessionsRatingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsAsync(string sessionId, string studentId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Set<SessionsRating>()
            .FirstOrDefaultAsync(r => r.SessionId == sessionId && r.StudentId == studentId, cancellationToken);

        return result is not null;
    }
}
