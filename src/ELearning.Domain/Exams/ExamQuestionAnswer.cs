using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Exams;
public sealed class ExamQuestionAnswer
{
    private ExamQuestionAnswer() { }
    public ExamQuestionAnswer(
        int userQuizId,
        int questionId,
        string? answer)
    {
        UserQuizId = userQuizId;
        QuestionId = questionId;
        Answer = answer;
    }
    public int UserQuizId { get; private set; }
    public int QuestionId { get; private set; }
    public string? Answer { get; private set; }
}
