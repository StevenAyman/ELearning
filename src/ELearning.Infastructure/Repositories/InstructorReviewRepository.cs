using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Reviews;

namespace ELearning.Infastructure.Repositories;
public sealed class InstructorReviewRepository : Repository<InstructorsReview>, IInstructorReviewRepository
{
    public InstructorReviewRepository(AppDbContext dbContext) : base(dbContext)
    {    
    }
}
