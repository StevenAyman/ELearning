using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;

namespace ELearning.Domain.Shared.Specifications.ExamQuestionSpecifications;
public class QuestionWIthAnswersSpecifications : BaseSpecifications<ExamQuestion>
{
    private readonly List<Expression<Func<ExamQuestion, object>>> _includes = new();
    public QuestionWIthAnswersSpecifications(Expression<Func<ExamQuestion, bool>> filter) : base(filter)
    {
        _includes.Add(q => q.McqQuestionAnswers);
        _includes.Add(q => q.CorrectAnswer);

    }
}
