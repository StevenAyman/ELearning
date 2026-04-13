using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Enrollments;

namespace ELearning.Infastructure.Repositories;
public sealed class UserQuizRepository : Repository<UserQuiz>, IUserQuizRepository
{
    public UserQuizRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
