using ELearning.Infastructure.Outbox;
using Hangfire;
using Microsoft.Extensions.Options;

namespace ELearning.Api.Extensions;

public static class BackgroundJobsExtension
{
    public static WebApplication UseBackgroundJob(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var outboxOptions = scope.ServiceProvider.GetRequiredService<IOptions<OutboxOptions>>().Value;

        scope.ServiceProvider.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IProcessOutboxMessageJob>("outbox-processor",
            options => options.ExecuteAsync(),
            outboxOptions.Schedule);

        return app;
    }
}
