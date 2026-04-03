using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared.Specifications;
public class BaseSpecifications<TEntity> : IBaseSpecifications<TEntity>
{
    public Expression<Func<TEntity, bool>> Filter { get; }

    public BaseSpecifications(Expression<Func<TEntity,bool>> filter)
    {
        Filter = filter;
    }
}
