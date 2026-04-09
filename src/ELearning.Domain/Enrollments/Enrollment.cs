using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Enrollments;
public sealed class Enrollment : BaseEntity
{
    private readonly List<VideoViewTank> _tanks = new();
    private Enrollment() { }

    public Enrollment(string id) : base(id)
    {
    }

    public string StudentId { get; private set; }
    public string SessionId { get; private set; }
    public string PurchaseId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public string? QuizId { get; private set; }
    public int? MaxTries { get; private set; }
    public int? RemainingTries { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public IReadOnlyList<VideoViewTank> Tanks => _tanks;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool HasQuiz => QuizId is not null;

    public static Enrollment Create(
        string id,
        string studentId,
        string sessionId,
        string PurchaseId,
        IEnumerable<Video> videos,
        DateTime utcNow,
        string? quizId,
        int? maxTries)
    {
        if (string.IsNullOrWhiteSpace(quizId) && maxTries.HasValue)
        {
            throw new ApplicationException("You can't set max tries if session doesn't have quiz");
        }

        if (quizId is not null && !maxTries.HasValue)
        {
            maxTries = 3;
        }

        var enrollment = new Enrollment(id)
        {
            StudentId = studentId,
            SessionId = sessionId,
            PurchaseId = PurchaseId,
            Status = EnrollmentStatus.Pending,
            ExpiresAt = utcNow.AddDays(7)
        };

        if (!string.IsNullOrWhiteSpace(quizId))
        {
            enrollment.QuizId = quizId;
            enrollment.MaxTries = maxTries;
        }

        foreach (var video in videos)
        {
            enrollment._tanks.Add(VideoViewTank.Create($"vt_{Guid.CreateVersion7()}", id, video));
        }

        return enrollment;
    }

    public Result CanAccess(string videoId)
    {
        if (IsExpired)
        {
            return Result.Failure(EnrollmentErrors.SessionExpired);
        }

        var tank = GetTank(videoId);
        if (tank is null)
        {
            return Result.Failure(EnrollmentErrors.VideoNotBelong);
        }

        if (!tank.IsUnlocked)
        {
            return Result.Failure(EnrollmentErrors.VideoNotUnlocked);
        }

        if (!tank.CanWatch)
        {
            return Result.Failure(EnrollmentErrors.VideoReachLimit);
        }

        return Result.Success();
    }

    private VideoViewTank GetTank(string videoId)
    {
        return _tanks.FirstOrDefault(t => t.VideoId == videoId);
    }

    public Result<VideoViewTank> OpenVideo(string videoId)
    {
        var access = CanAccess(videoId);
        if (access.IsFailure)
        {
            return Result<VideoViewTank>.Failure(access.Error!);
        }

        var tank = GetTank(videoId);
        return Result<VideoViewTank>.Succuss(tank);
    }

    public Result StartOver(string videoId)
    {
        var access = CanAccess(videoId);
        if (access.IsFailure)
        {
            return access;
        }

        var tank = GetTank(videoId);
        tank.ResetPosition();
        return Result.Success();
    }

    public Result<(VideoViewTank tank, bool isConsumed, bool isCompleted)> RecordHearbeats(
        string videoId,
        int lastPositionInSeconds,
        int durationInSeconds,
        DateTime utcNow)
    {
        var access = CanAccess(videoId);
        if (access.IsFailure)
        {
            return Result<(VideoViewTank, bool, bool)>.Failure(access.Error!);
        }

        var tank = GetTank(videoId);
        (bool isConsumed, bool isCompleted) = tank.DrainTank(durationInSeconds, lastPositionInSeconds, utcNow);

        return Result<(VideoViewTank, bool, bool)>.Succuss((tank, isConsumed, isCompleted));
    }
    

    public void UnlockDependents(string videoId, IEnumerable<Video> videos)
    {
        var dependencies = videos.Where(v => v.HasPrerequisite && v.PrerequisiteId == videoId);

        foreach (var video in dependencies)
        {
            GetTank(video.Id).Unlock();
        }
    }

    public double CalculateSessionProgress(IEnumerable<Video> videos)
    {
        var videosWithProgress = videos.Where(v => v.IsCountedInSessionProgression).ToList();

        if (!videosWithProgress.Any())
        {
            return 0;
        }

        var totalVideosDuration = videosWithProgress.Sum(v => v.DurationInSeconds);

        if (totalVideosDuration == 0)
        {
            return 0;
        }

        var videosProgresses = videosWithProgress.Sum(v =>
        {
            var tank = GetTank(v.Id);
            if (tank is null)
            {
                return 0;
            }

            return tank.CalculateVideoProgressInSeconds();
        });

        return  (double)videosProgresses / totalVideosDuration * 100;

    }

    public void ExtendByFixedDate(DateTime newExpireDate)
    {
        if (newExpireDate <= DateTime.UtcNow)
        {
            throw new ApplicationException("Invalid expire date. expire date should be in the future");
        }

        ExpiresAt = newExpireDate;
    }

    public void ExtendByDays(int days, DateTime utcNow)
    {
        if (days < 0)
        {
            throw new ApplicationException("Expire days should be more than zero");
        }

        ExpiresAt = utcNow.AddDays(days);
    }
}
