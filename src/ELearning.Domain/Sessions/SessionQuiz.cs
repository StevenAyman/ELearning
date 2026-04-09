using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Sessions;
public sealed class SessionQuiz : BaseEntity
{
    private readonly List<ExamQuestion> _questions = new List<ExamQuestion>();
    private SessionQuiz () { }
    private SessionQuiz(
        string id,
        string instructorId,
        string subjectId,
        Percentage passingPercentage,
        Title title,
        int totalGrades,
        int maximumTries,
        int totalQuestions,
        DateTime createdAt,
        IEnumerable<ExamQuestion> questions) : base(id)
    {
        PassingPercentage = passingPercentage;
        Title = title;
        TotalGrades = totalGrades;
        MaximumTries = maximumTries;
        InstructorId = instructorId;
        SubjectId = subjectId;
        _questions = questions.ToList();
        TotalQuestions = totalQuestions;
        CreatedAtUtc = createdAt;
        Status = QuizStatus.Active;
        
    }
    public string InstructorId { get; private set; }
    public string SubjectId { get; private set; }
    public Title Title { get; private set; }
    public int TotalGrades { get; private set; }
    public int TotalQuestions { get; private set; }
    public int DurationInSeconds { get; private set; }
    public Percentage PassingPercentage { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public int MaximumTries { get; private set; }
    public QuizStatus Status { get; private set; }
    public IReadOnlyList<ExamQuestion> Questions => _questions;

    public static SessionQuiz Create(
        string id,
        string instructorId,
        string subjectId,
        Title title,
        int durationInSeconds,
        int totalGrades,
        int totalQuestions,
        Percentage passingPercentage,
        int maximumTries,
        DateTime createdAtUtc,
        IEnumerable<ExamQuestion> questions)
    {
        if (questions is null)
        {
            throw new ApplicationException("Sorry any quiz should contain at least 1 question");
        }
        questions = questions.ToList();
        if (!questions.Any())
        {
            throw new ApplicationException("Sorry any quiz should contain at least 1 question");
        }

        if (maximumTries < 1)
        {
            throw new ApplicationException("Maximum tries should be at least 1");
        }

        if (totalGrades <= 0)
        {
            throw new ApplicationException("Total grades should be more than 0");
        }

        if (durationInSeconds <= 0)
        {
            throw new ApplicationException("Invalid quiz duration");
        }

        var isALlMcq = questions.All(q => q.Type == ExamQuestionType.Mcq);
        if (!isALlMcq)
        {
            throw new ApplicationException("The quiz should contain only MCQ questions");
        }

        if (passingPercentage.Value <= 0 || passingPercentage.Value > 1)
        {
            throw new ApplicationException("Invalid passing percentage");
        }

        var questionsGradesSum = (int)questions.Sum(q => q.Grade);
        if (questionsGradesSum != totalGrades)
        {
            throw new ApplicationException("Sorry question grades sum not equal total grades");
        }

        if (totalQuestions != questions.Count())
        {
            throw new ApplicationException("Total questions doesn't match the given questions count");
        }

        return new SessionQuiz(id, instructorId, subjectId, passingPercentage, title, totalGrades, maximumTries, totalQuestions, createdAtUtc, questions);
    }

    public void HideQuiz()
    {
        if (Status != QuizStatus.Active)
        {
            throw new ApplicationException("Quiz is already hidden");
        }

        Status = QuizStatus.Hidden;
    }

    public void PublishQuiz()
    {
        if (Status != QuizStatus.Hidden)
        {
            throw new ApplicationException("The quiz is already published");
        }

        Status = QuizStatus.Active;
    }


}
