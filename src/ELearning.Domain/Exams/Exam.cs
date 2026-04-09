using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Exams;
public sealed class Exam : BaseEntity
{
    private List<ExamQuestion> _questions = new();
    private Exam() { }
    public Exam(
        string id,
        string instructorId,
        string subjectId,
        Title title,
        int questionsNumber,
        int totalGrades,
        int durationInSeconds,
        Money price,
        ExamType examType,
        IEnumerable<ExamQuestion> examQuestions,
        ExamResultType resultDisplay) : base(id)
    {
        Title = title;
        QuestionsNumber = questionsNumber;
        TotalGrades = totalGrades;
        Price = price;
        ExamType = examType;
        SubjectId = subjectId;
        InstructorId = instructorId;
        _questions = examQuestions.ToList();
        Status = ExamStatus.Draft;
        DurationInSeconds = durationInSeconds;
        ResultDisplay = resultDisplay;
    }

    public Title Title { get; private set; }
    public int QuestionsNumber { get; private set; }
    public int TotalGrades { get; private set; }
    public Money Price { get; private set; }
    public ExamType ExamType { get; private set; }
    public string InstructorId { get; private set; }
    public string SubjectId { get; private set; }
    public ExamStatus Status { get; private set; }
    public ExamResultType ResultDisplay { get; private set; }
    public int DurationInSeconds { get; private set; }
    public DateTime? PublishedAtUtc { get; private set; }
    public IReadOnlyList<ExamQuestion> Questions => _questions;

    public static Exam Create(
        string id,
        string instructorId,
        string subjectId,
        ExamType examType,
        Title title,
        IEnumerable<ExamQuestion> questions,
        int totalGrades,
        Money price,
        int questionsNumber,
        int durationInSeconds,
        ExamResultType resultType)
    {
        if (questions is null)
        {
            throw new ApplicationException("Questions shouldn't be null or empty");
        }

        questions = questions.ToList();
        if (!questions.Any())
        {
            throw new ApplicationException("Invalid operation exam should has at least one question");
        }

        if (questions.Count() != questionsNumber)
        {
            throw new ApplicationException("Invalid questions number");
        }

        if (totalGrades <= 0)
        {
            throw new ApplicationException("Invalid grades");
        }

        var totalQuestionsGrades = (int) Math.Ceiling(questions.Sum(q => q.Grade));

        if (totalQuestionsGrades != totalGrades)
        {
            throw new ApplicationException("Total grades not matching all questions grade");
        }

        if (durationInSeconds <= 0)
        {
            throw new ApplicationException("Duration is invalid");
        }

        if (questions.Any(q => q.Type == ExamQuestionType.Written) &&
            resultType == ExamResultType.QuickView)
        {
            throw new ApplicationException("Sorry exam can't be displayed after submit because it contains written questions");
        }

        return new Exam(id, instructorId, subjectId, title, questionsNumber, totalGrades, durationInSeconds, price, examType, questions, resultType);

    }

    public void Publish(DateTime utcNow)
    {
        if (Status != ExamStatus.Draft)
        {
            throw new ApplicationException("Exam is already published");
        }

        Status = ExamStatus.Published;
        PublishedAtUtc = utcNow;
    }

    public void Unpublish()
    {
        if (Status != ExamStatus.Published)
        {
            throw new ApplicationException("Exam is already unpublished");
        }

        Status = ExamStatus.Draft;
        PublishedAtUtc = null!;
    }

    public void UpdateExam(
        Title title,
        int totalGrades,
        Money price,
        int questionsNumber,
        IEnumerable<ExamQuestion> questions)
    {
        if (Status != ExamStatus.Draft)
        {
            throw new ApplicationException("Can't update published exam");
        }

        if (questions is null)
        {
            throw new ApplicationException("Questions can't by null or empty");
        }
        questions = questions.ToList();
        if (!questions.Any())
        {
            throw new ApplicationException("Questions Should be at least one");
        }

        if (totalGrades <= 0)
        {
            throw new ApplicationException("Invalid grades");
        }

        if (questionsNumber <= 0)
        {
            throw new ApplicationException("Invalid questions number");
        }

        if (questions.Count() != questionsNumber)
        {
            throw new ApplicationException("Questions number doesn't match");
        }

        var questionsGradesSum = (int) questions.Sum(q => q.Grade);
        if (questionsGradesSum != totalGrades)
        {
            throw new ApplicationException("Total grades doesn't match sum of questions grades");
        }

        TotalGrades = totalGrades;
        _questions = questions.ToList();
        Title = title;
        Price = price;
        QuestionsNumber = questionsNumber;
    }
}
