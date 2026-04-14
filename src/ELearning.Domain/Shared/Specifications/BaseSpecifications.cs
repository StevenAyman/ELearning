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

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
    public List<string> StringIncludes { get; } = new();

    public BaseSpecifications(Expression<Func<TEntity,bool>> filter)
    {
        Filter = filter;
    }

    protected void AddIncludes(Expression<Func<TEntity, object>> include)
    {
        Includes.Add(include);
    }

    protected void AddStringIncludes(string include)
    {
        StringIncludes.Add(include);
    }
}
