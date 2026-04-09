using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Enrollments;
public sealed class VideoViewTank : BaseEntity
{
    private VideoViewTank() { }
    private VideoViewTank(string id) : base(id) 
    {
    }
    public string EnrollmentId { get; private set; }
    public string VideoId { get; private set; }
    public int? MaxViewCount { get; private set; }
    public int UsedViewCount { get; private set; }
    public int DurationThreshold { get; private set; }
    public int VideoDurationSeconds { get; private set; }
    public int TankSecondsCapacity { get; private set; }
    public int RemainingTankSeconds { get; private set; }
    public bool IsUnlocked { get; private set; }
    public bool IsCompleted { get; private set; }
    public int LastPositionInSeconds { get; private set; }
    public int BestPositionInSeconds { get; private set; }
    public DateTime? LastActiveAtUtc { get; private set; }

    public bool IsUnlimited => MaxViewCount is null;
    public bool CanWatch => IsUnlimited || UsedViewCount < MaxViewCount;
    public int RemainingViews => IsUnlimited ? int.MaxValue : MaxViewCount!.Value - UsedViewCount;
    public int DrainedSeconds => TankSecondsCapacity - RemainingTankSeconds;
    
    public int CalculateVideoProgressInSeconds()
    {
        if (!IsCompleted)
        {
            return Math.Min(BestPositionInSeconds, DrainedSeconds);
        }

        return VideoDurationSeconds;
    }

    public void Unlock() => IsUnlocked = true;

    public (bool isConsumed, bool isCompleted) DrainTank(int contentSeconds, int lastPositionSeconds, DateTime utcNow)
    {
        RemainingTankSeconds = Math.Max(0, RemainingTankSeconds - contentSeconds);
        LastPositionInSeconds = lastPositionSeconds;
        LastActiveAtUtc = utcNow;

        bool isConsumed = false;
        bool isCompleted = false;

        if (RemainingTankSeconds == 0 && !IsUnlimited)
        {
            UsedViewCount++;
            isConsumed = true;

            if (CanWatch)
            {
                RemainingTankSeconds = TankSecondsCapacity;
            }
        }

        if (!IsCompleted && UsedViewCount == 1)
        {
            var drainedThreshold = DrainedSeconds >= DurationThreshold;
            var positionThreshold = lastPositionSeconds >= DurationThreshold;

            if (drainedThreshold &&  positionThreshold)
            {
                IsCompleted = true;
                isCompleted = true;
            }
        }

        return (isConsumed, isCompleted);
    }

    public void ResetPosition() => LastPositionInSeconds = 0;

    public static VideoViewTank Create(string id, string enrollmentId, Video video)
    {
        var thresholdSeconds = (int) (video.DurationInSeconds * video.ThresholdPercentage.Value);
        var tankSeconds = (int)(video.DurationInSeconds * 1.5);

        return new VideoViewTank(id)
        {
            EnrollmentId = enrollmentId,
            VideoId = video.Id,
            DurationThreshold = thresholdSeconds,
            MaxViewCount = video.MaxViewCount,
            TankSecondsCapacity = tankSeconds,
            RemainingTankSeconds = tankSeconds,
            UsedViewCount = 0,
            IsCompleted = false,
            VideoDurationSeconds = video.DurationInSeconds,
            IsUnlocked = !video.HasPrerequisite,
            LastPositionInSeconds = 0,
            BestPositionInSeconds = 0,
        };
    }
}
