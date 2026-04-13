using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Infastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Infastructure.Repositories;
public class Repository<T>(AppDbContext dbContext) where T : class
{
    protected AppDbContext _dbContext = dbContext;

    public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
         => await _dbContext.Set<T>().FindAsync([id], cancellationToken);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Set<T>().ToListAsync(cancellationToken);

    public async Task<T?> GetWithSpecAsync(
        IBaseSpecifications<T> specs, 
        CancellationToken cancellationToken = default)
            => await GetEvaluatedQuery(specs).FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(
        IBaseSpecifications<T> specs,
        CancellationToken cancellationToken = default)
            => await GetEvaluatedQuery(specs).ToListAsync(cancellationToken);

    public void Update(T entity)
    {
        _dbContext.Update(entity);
    }

    public void Add(T entity)
    {
        _dbContext.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }


    private IQueryable<T> GetEvaluatedQuery(IBaseSpecifications<T> specs)
        => SpecificationEvaluator<T>.BuildQuery(_dbContext.Set<T>(), specs);

}
