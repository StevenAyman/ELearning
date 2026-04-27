using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Users;
public interface IUserRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(TEntity entity, User user)?> GetUserWithProfileAsync(string id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetWithSpecAsync(IBaseSpecifications<TEntity> specs, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
