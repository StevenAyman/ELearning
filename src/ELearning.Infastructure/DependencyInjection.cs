using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Cache;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Data;
using ELearning.Domain.Classes;
using ELearning.Domain.Discounts;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Exams;
using ELearning.Domain.Instructors;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings;
using ELearning.Domain.Reviews;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using ELearning.Domain.Users;
using ELearning.Infastructure.Caching;
using ELearning.Infastructure.Clock;
using ELearning.Infastructure.Data;
using ELearning.Infastructure.Data.Authorization;
using ELearning.Infastructure.Outbox;
using ELearning.Infastructure.Repositories;
using ELearning.Infastructure.Services.KeycloakService;
using ELearning.Infastructure.Services.KeycloakService.DTOs;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Refit;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace ELearning.Infastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        AddPersistance(services, configuration);
        AddRepositories(services);
        AddCaching(services, configuration);
        AddBackgroundJobs(services, configuration);
        AddRefit(services, configuration);


        return services;
    }

    public static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
        });
        services.AddFusionCache()
            .WithDefaultEntryOptions(options =>
            {
                options.Duration = TimeSpan.FromMinutes(5);
                options.IsFailSafeEnabled = true;
            })
            .WithSerializer(new FusionCacheSystemTextJsonSerializer())
            .WithDistributedCache(sp => sp.GetRequiredService<IDistributedCache>())
            .WithBackplane(new RedisBackplane(new RedisBackplaneOptions
            {
                Configuration = redisConnection
            }))
            .AsHybridCache();

        services.AddSingleton<ICacheService, CacheService>();
    }

    public static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        // Repositories Start
        services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ISessionQuizRepository, SessionQuizRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IVideoQuestionRepository, VideoQuestionRepository>();
        services.AddScoped<IQuestionAnswerVoteRepository, QuestionAnswerVoteRepository>();
        services.AddScoped<IInstructorReviewRepository, InstructorReviewRepository>();
        services.AddScoped<IInstructorsRatingRepository, InstructorsRatingRepository>();
        services.AddScoped<ISessionsRatingRepository, SessionsRatingRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<IPaidCodeRepository, PaidCodeRepository>();
        services.AddScoped<IPurchaseMethodRepository, PurchaseMethodRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IExamQuestionRepository, ExamQuestionRepository>();
        services.AddScoped<IExamEnrollmentRepository, ExamEnrollmentRepository>();
        services.AddScoped<IExamQuestionAnswerRepository, ExamQuestionAnswerRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IUserQuizRepository, UserQuizRepository>();
        services.AddScoped<ICodeApplicableAreaRepository, CodeApplicableAreaRepository>();
        services.AddScoped<ICodeAreasRepository, CodeAreasRepository>();
        services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ILearningClassRepository, LearningClassRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IInstructorSubjectRepository, InstructorSubjectRepository>();
        // Repositories End

        // Read Services Start
        services.AddScoped<IUserReadService, UserReadService>();
        services.AddScoped<ISubjectReadService, SubjectReadService>();
        services.AddScoped<IInstructorReadService, InstructorReadService>();
        services.AddScoped<ISessionReadService, SessionReadService>();
        services.AddScoped<IAssistantReadService, AssistantReadService>();
        services.AddScoped<IClassReadService,  ClassReadService>();
        // Read Services End
    }

    public static void AddPersistance(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppDbContext>(options =>
        {

            options.UseSqlServer(connectionString)
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

    }
    
    public static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                                throw new ArgumentNullException(nameof(configuration)); 

        services.AddOptions<OutboxOptions>().BindConfiguration(OutboxOptions.SectionName);
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString, new SqlServerStorageOptions()
            {
                PrepareSchemaIfNecessary = true,

                SchemaName = "Hangfire"
            });

        });

        services.AddHangfireServer(config =>
        {
            config.SchedulePollingInterval = TimeSpan.FromSeconds(1);
        });

        services.AddScoped<IProcessOutboxMessageJob, ProcessOutboxMessageJob>();
    }

    public static void AddRefit(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KeycloakTokenRequest>()
            .Bind(configuration.GetSection("KeycloakOptions"));

        services.AddOptions<KeycloakOptions>()
            .Bind(configuration.GetSection("KeycloakOptions"));

        var baseUrl = configuration["KeycloakOptions:BaseUrl"];

        services.AddTransient<KeycloakAccessTokenHandler>();
        services.AddTransient<KeycloakAdminApiService>();

        services.AddRefitClient<IKeycloakAuthApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl!));

        services.AddRefitClient<IKeycloakAdminApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl!))
            .AddHttpMessageHandler<KeycloakAccessTokenHandler>()
            .AddResilienceHandler("keycloak-retry", pipeline =>
            {
                pipeline.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 5,
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(rm => rm.StatusCode == HttpStatusCode.NotFound)
                });
            });


    }

    public static async Task<WebApplication> ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            logger.LogInformation("Starting to migrate all pending migrations.");
            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Completed all pending migration successfully");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An error has occurred while trying to apply migrations");
        }

        return app;
    } 
}
