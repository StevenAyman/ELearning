using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Users;
public interface IUserRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetById(string id);
    Task<IEnumerable<TEntity>> GetAll();
    void Add(TEntity instructor);
    void Update(TEntity instructor);
    void Delete(string id);
}
