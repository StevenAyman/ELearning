using ELearning.Api.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapControllers();

await app.RunAsync();
