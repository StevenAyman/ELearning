using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Sessions;
public sealed class Video : BaseEntity
{
    private Video() { }

    public Video(
        string id,
        Title title,
        string url,
        VideoOrder order,
        int durationInSeconds,
        Percentage thresholdPercentage,
        bool isInProgression,
        bool hasPrerequisite,
        string? prerequisiteId = null,
        int? maxViewCount = null) : base(id) 
    {
        Title = title;
        Url = url;
        Order = order;
        DurationInSeconds = durationInSeconds;
        ThresholdPercentage = thresholdPercentage;
        IsCountedInSessionProgression = isInProgression;
        HasPrerequisite = hasPrerequisite;
        PrerequisiteId = prerequisiteId;
        MaxViewCount = maxViewCount;
    }

    public Title Title { get; private set; }
    public string Url { get; private set; }
    public VideoOrder Order { get; private set; }
    public int DurationInSeconds { get; private set; }
    public Percentage ThresholdPercentage { get; private set; }
    public int? MaxViewCount { get; private set; }
    public bool HasPrerequisite { get; private set; }
    public string? PrerequisiteId { get; private set; }
    public bool IsCountedInSessionProgression { get; private set; }
    public bool IsUnLimited => MaxViewCount is null;
    public int ThresholdSeconds => (int)(ThresholdPercentage.Value * DurationInSeconds);

    public static Video CreateVideo(string id,
        Title title,
        string url,
        VideoOrder order,
        int durationInSeconds,
        Percentage thresholdPercentage,
        bool isInProgression,
        bool hasPrerequisite,
        string? prerequisiteId,
        int? maxViewCount)
    {
        if (!hasPrerequisite && prerequisiteId is not null)
        {
            throw new ApplicationException("Can't set the prerequisite while video has no prerequisite");
        }

        if (hasPrerequisite && prerequisiteId is null)
        {
            throw new ApplicationException("Video has a prerequisite you should define it");
        }

        return new Video(id, title, url, order, durationInSeconds, thresholdPercentage, isInProgression, hasPrerequisite, prerequisiteId, maxViewCount);
    }
    public void UpdateTitle(Title newTitle)
    {
        if (newTitle is not null && !string.IsNullOrWhiteSpace(newTitle.Value))
        {
            Title = newTitle;
            return;
        }
        throw new ApplicationException("Video title shouldn't be empty");
    }
    public void UpdateUrl(string newUrl)
    {
        if (!string.IsNullOrWhiteSpace(newUrl))
        {
            Url = newUrl;
            return;
        }

        throw new ApplicationException("Video url shouldn't be empty");
    }
    public void UpdateOrder(VideoOrder order)
    {
        Order = order;
    }

    public void UpdateDetails(int durationInSeconds, Percentage thresholdPercentage, bool isInProgression, int maxViewCount)
    {
        DurationInSeconds = durationInSeconds;
        ThresholdPercentage = thresholdPercentage;
        IsCountedInSessionProgression = isInProgression;
        MaxViewCount = maxViewCount;
    }
    public void UpdatePrerequisite(bool hasPrerequisite, string? prerequisiteId)
    {
        if (!hasPrerequisite && !string.IsNullOrWhiteSpace(prerequisiteId))
        {
            throw new ApplicationException("Sorry to set prerequisite video should has prerequisite (true)");
        }

        if (hasPrerequisite && string.IsNullOrWhiteSpace(prerequisiteId))
        {
            throw new ApplicationException("You should provide the prerequisite");
        }

        HasPrerequisite = hasPrerequisite;
        PrerequisiteId = prerequisiteId;
    }
}
