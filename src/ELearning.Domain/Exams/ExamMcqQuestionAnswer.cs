namespace ELearning.Domain.Exams;

public sealed class ExamMcqQuestionAnswer
{
    private ExamMcqQuestionAnswer() { }
    public ExamMcqQuestionAnswer(int answerId, string answer)
    {
        AnswerId = answerId;
        Answer = answer;
    }

    public int AnswerId { get; private set; }
    public string Answer { get; private set; }

}
