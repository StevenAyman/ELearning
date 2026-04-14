using ELearning.Api.Extensions;
using ELearning.Application;
using ELearning.Infastructure;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddInfastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHangfireDashboard("/hangfire");
}

await app.ApplyMigrationsAsync();

app.UseBackgroundJob();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapControllers();

await app.RunAsync();
