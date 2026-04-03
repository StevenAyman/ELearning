using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Sessions;
public static class SessionErrors
{
    public static readonly Error EmptySession = new(
        "Session.NoVideos",
        "Failed to publish this session, session should contain at least one video.");

}
