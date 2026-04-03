using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared.Specifications;
public interface IBaseSpecifications<TEntity>
{
    Expression<Func<TEntity, bool>> Filter { get; } 
}
