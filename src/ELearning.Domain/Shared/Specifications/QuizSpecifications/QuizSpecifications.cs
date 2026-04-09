using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Domain.Shared.Specifications.QuizSpecifications;
public sealed class QuizSpecifications : BaseSpecifications<SessionQuiz>
{
    public QuizSpecifications(Expression<Func<SessionQuiz, bool>> filter) : base(filter)
    {
        
    }
}
