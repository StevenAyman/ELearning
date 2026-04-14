using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Ratings;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Infastructure.Repositories;
public sealed class InstructorsRatingRepository : Repository<InstructorsRating>, IInstructorsRatingRepository
{
    public InstructorsRatingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsAsync(string instructorId, string studentId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Set<InstructorsRating>()
            .FirstOrDefaultAsync(r => r.InstructorId == instructorId && r.StudentId == studentId, cancellationToken);

        return result is not null;
    }
}
