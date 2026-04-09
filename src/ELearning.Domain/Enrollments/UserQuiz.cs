using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Enrollments;
public sealed class UserQuiz : BaseEntity
{
    private UserQuiz() { }
    public UserQuiz(
        string id,
        string enrollmentId,
        string quizId,
        double totalScore,
        Percentage threshold,
        int durationInSeconds,
        DateTime issuedAt) : base(id)
    {
        QuizId = quizId;
        TotalScore = totalScore;
        Threshold = threshold;
        EnrollmentId = enrollmentId;
        DurationInSeconds = durationInSeconds;
        IssuedAtUtc = issuedAt;
        Score = null;
    }

    public string EnrollmentId { get; private set; }
    public string QuizId { get; private set; }
    public double? Score { get; private set; }
    public double TotalScore { get; private set; }
    public Percentage Threshold { get; private set; }
    public int DurationInSeconds { get; private set; }
    public DateTime IssuedAtUtc { get; private set; }
    public int FinishTimeInSeconds { get; private set; }

    public DateTime ExpiresAt => IssuedAtUtc.AddSeconds(DurationInSeconds + 5);

    public void UpdateFinishTime(int seconds)
    {
        if (seconds <= 0)
        {
            throw new ApplicationException("Invalid period");
        }
        FinishTimeInSeconds = seconds;
    }

    public void UpdateScore(double score)
    {
        if (score < 0)
        {
            throw new ApplicationException("Score can't be less than zero");
        }

        Score = score;
    }
}
