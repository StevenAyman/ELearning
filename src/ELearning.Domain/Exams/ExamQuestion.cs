using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;

namespace ELearning.Domain.Exams;
public sealed class ExamQuestion
{
    private readonly List<ExamMcqQuestionAnswer> _answers = new List<ExamMcqQuestionAnswer>();
    private ExamQuestion(Title title, double grade, string? question = null)
    {
        Title = title;
        Question = question;
        Type = ExamQuestionType.Written;
        Grade = grade;
    }
    private ExamQuestion(Title title, IEnumerable<ExamMcqQuestionAnswer> answers, ExamMcqQuestionAnswer correctAnswer, double grade, string? question = null)
    {
        Title = title;
        Type = ExamQuestionType.Mcq;
        _answers = answers.ToList();
        CorrectAnswer = correctAnswer;
        Question = question;
        Grade = grade;
    }

    public int Id { get; private set; }
    public Title Title { get; private set; }
    public string? Question { get; private set; }
    public ExamQuestionType Type { get; private set; }
    public ExamMcqQuestionAnswer? CorrectAnswer { get; private set; }
    public double Grade { get; private set; }
    public IReadOnlyList<ExamMcqQuestionAnswer>? McqQuestionAnswers => _answers;

    public static ExamQuestion CreateMcqQuestion(
        Title title, 
        double grade,
        IEnumerable<ExamMcqQuestionAnswer> answers, 
        ExamMcqQuestionAnswer correct, 
        string? question = null)
    {
        if (answers is null)
        {
            throw new ApplicationException("Answers shouldn't be null or empty");
        }
        answers = answers.ToList();

        if (!answers.Any())
        {
            throw new ApplicationException("Invalid questions. Mcq question should have set of answers");
        }

        if (!answers.Contains(correct))
        {
            throw new ApplicationException("Correct answer should be included in question answers");
        }

        return new ExamQuestion(title, answers, correct, grade, question);
    }

    public static ExamQuestion CreateWrittenQuestion(Title title, double grade, string? question = null)
    {
        return new ExamQuestion(title, grade, question);
    }
}
