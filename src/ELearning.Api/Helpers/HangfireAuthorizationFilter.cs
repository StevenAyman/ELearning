using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace ELearning.Api.Helpers;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}
