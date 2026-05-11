using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Behaviors;
using ELearning.Domain.Purchases;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<IApplicationMarker>();

            config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));

            config.AddOpenBehavior(typeof(QueryCacheBehavior<,>));
            
            config.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));

        });

        services.AddTransient<CodeGenerationDomainService>();

        return services;
    }
}
