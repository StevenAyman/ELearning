using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Domain.Discounts;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Exams;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings;
using ELearning.Domain.Reviews;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using ELearning.Domain.Users;
using ELearning.Infastructure.Data;
using ELearning.Infastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning.Infastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppDbContext>(options =>
        {

            options.UseSqlServer(connectionString)
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

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
        // Repositories End

        return services;
    }
}
