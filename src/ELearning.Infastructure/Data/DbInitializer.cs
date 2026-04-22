using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Infastructure.Data.Authorization;

namespace ELearning.Infastructure.Data;
public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext _dbContext)
    {
        SeedRoles(_dbContext);


        await _dbContext.SaveChangesAsync();
    }
    
    private static void SeedRoles(AppDbContext _dbContext)
    {
        if (!_dbContext.Set<Role>().Any())
        {
            var roles = new List<Role>()
            {
                new Role("Instructor"),
                new Role("Admin"),
                new Role("Student"),
                new Role("Assistant")
            };

            _dbContext.Set<Role>().AddRange(roles);
        }
    }
}
