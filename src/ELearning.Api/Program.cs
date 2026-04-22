using ELearning.Api.Extensions;
using ELearning.Api.Helpers;
using ELearning.Application;
using ELearning.Infastructure;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddInfastructure(builder.Configuration)
    .AddApplication();



builder.Services.AddCors(options =>
{
    options.AddPolicy("SwaggerPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
    //app.MapOpenApi();
}

await app.ApplyMigrationsAsync();

app.UseBackgroundJob();
app.UseCors("SwaggerPolicy");
//app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages();

app.UseExceptionHandler();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireAuthorizationFilter()]
});

app.MapControllers();

await app.RunAsync();
