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
        VideoOrder order) : base(id) 
    {
        Title = title;
        Url = url;
        Order = order;
    }

    public Title Title { get; private set; }
    public string Url { get; private set; }
    public VideoOrder Order { get; private set; }

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
}
