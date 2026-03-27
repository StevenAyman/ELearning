using ELearning.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
