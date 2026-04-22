using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace ELearning.Infastructure.Data.Authorization;
public sealed class PermissionService(AppDbContext dbContext, HybridCache cache) : IPermissionService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly HybridCache _cache = cache;
    public async Task<Role> GetRoleWithPermissionsAsync(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"roleWithPermissions:{id}";
        var role = await _cache.GetOrCreateAsync(
            cacheKey,
            async (token) =>
            {
                return await _dbContext.Set<Role>()
                    .Include(r => r.Permissions)
                    .FirstOrDefaultAsync(r => r.Id == id, token);
            }, cancellationToken: cancellationToken);

        return role;
    }

    public async Task<IReadOnlyList<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Roles";
        var roles = await _cache.GetOrCreateAsync(
            cacheKey,
            async token =>
            {
                return await _dbContext.Set<Role>().ToListAsync(token);
            }, cancellationToken: cancellationToken);

        return roles;
    }
}
