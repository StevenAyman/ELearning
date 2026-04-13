using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Infastructure.Specifications;
public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> BuildQuery(IQueryable<T> inputQuery, IBaseSpecifications<T> specs)
    {
        var query = inputQuery;
        if (specs is null)
        {
            throw new ArgumentException("Sorry can't proceed with empty specifications");
        }

        if (specs.Filter is not null)
        {
            query = query.Where(specs.Filter);
        }

        if (specs.Includes is not null && specs.Includes.Any())
        {

            query = specs.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
        }

        if (specs.StringIncludes is not null && specs.StringIncludes.Any())
        {
            query = specs.StringIncludes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));
        }

        return query;
    }
}
