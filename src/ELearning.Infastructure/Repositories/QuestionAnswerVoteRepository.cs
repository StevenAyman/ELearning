using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;

namespace ELearning.Infastructure.Repositories;
public sealed class QuestionAnswerVoteRepository : Repository<QuestionAnswerVote>, IQuestionAnswerVoteRepository
{
    public QuestionAnswerVoteRepository(AppDbContext dbContext) : base(dbContext)
    {  
    }
}
