using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Infastructure.Repositories;
public sealed class UserRepository<T> : Repository<T>, IUserRepository<T> where T : BaseEntity
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(T entity, User user)?> GetUserWithProfileAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await (from e in _dbContext.Set<T>()
                            join u in _dbContext.Set<User>() on e.Id equals u.Id
                            where e.Id == id
                            select new { e, u }).SingleOrDefaultAsync(cancellationToken);

        return result?.e is null? null : (result.e, result.u);
    }
}
