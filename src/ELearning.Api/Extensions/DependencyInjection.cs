using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ELearning.Api.Extensions;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder app)
    {
        app.Services.AddControllers();
        app.Services.AddOpenApi();

        app.AddOpenTelemtery();
        return app;
    }

    public static WebApplicationBuilder AddOpenTelemtery(this WebApplicationBuilder app)
    {
        app.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(app.Environment.ApplicationName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation())
            .UseOtlpExporter();

        app.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
        });


        return app;
    }
}
