using ELearning.Api.BackgroundJobs;
using ELearning.Api.Exceptions;
using ELearning.Api.Helpers;
using ELearning.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace ELearning.Api.Extensions;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder app)
    {

        app.Services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        app.Services.AddTransient<LinkService>();

        app.Services.AddHttpContextAccessor();

        AddControllers(app);

        AddProblemDetails(app);
        
        AddSecurity(app);
        
        AddOpenTelemtery(app);
        
        AddSerilog(app);
        
        AddSwagger(app);

        AddBackgroundJobsServices(app);


        return app;
    }


    public static void AddControllers(WebApplicationBuilder app)
    {
        app.Services.AddControllers(options =>
        {
            options.ReturnHttpNotAcceptable = true;
            options.RespectBrowserAcceptHeader = true;
        })
        .AddXmlSerializerFormatters()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });

        app.Services.Configure<MvcOptions>(options =>
        {
            NewtonsoftJsonOutputFormatter formatter = options.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>().First();

            formatter.SupportedMediaTypes.Add(CustomMediaTypes.HateoasJson);
        });
    }
    public static void AddOpenTelemtery(WebApplicationBuilder app)
    {
        app.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(app.Environment.ApplicationName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
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
    }

    public static void AddSerilog(WebApplicationBuilder app)
    {
        app.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services);
        }, writeToProviders: true);
    }

    public static void AddSwagger(WebApplicationBuilder app)
    {
        //app.Services.AddOpenApi();

        app.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "ELearning",
                Version = "v1",

            });

            // form 2 to generate the swagger documentation
            foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.XML", SearchOption.TopDirectoryOnly))
            {
                options.IncludeXmlComments(filePath: name);
            }

            // second form to give Authorization with out whrite the world Bearer
            var securityScheme = new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    AuthorizationCode = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri(app.Configuration["Keycloak:AuthorizationUrl"]!),
                        TokenUrl = new Uri(app.Configuration["Keycloak:TokenUrl"]!),
                        Scopes = new Dictionary<string, string>
                        {
                             { "openid", "OpenID Connect scope" },
                             { "profile", "User profile" }
                        }
                    }
                }
            };

            var securityRequirement = new OpenApiSecurityRequirement
           {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Keycloak"
                         }
                     },
                     []
                 }
             };
            options.AddSecurityDefinition("Keycloak", securityScheme);
            options.AddSecurityRequirement(securityRequirement);

            string xmlFile = $"{typeof(DependencyInjection).Assembly.GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

        });

    }

    public static void AddSecurity(WebApplicationBuilder app)
    {

        app.Services.AddScoped<IClaimsTransformation, PermissionClaimsTransformation>();
        //app.Services.AddAuthorizationBuilder()
        //    .AddPolicy("users:read", policy => policy.RequirePermission(Permissions.UsersRead));
        app.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.Audience = app.Configuration["Keycloak:Audience"];
                config.MetadataAddress = app.Configuration["Keycloak:MetadataAddress"]!;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = app.Configuration["Keycloak:Issuer"],
                };

                config.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Put a breakpoint here!
                        var message = context.Exception.Message;
                        Console.WriteLine("Auth Failed: " + message);
                        Console.WriteLine($"Keys loaded: {context.Options.ConfigurationManager != null}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token Validated Successfully!");
                        return Task.CompletedTask;
                    }
                };
            });

        app.Services.AddAuthorization();
    }

    public static void AddProblemDetails(WebApplicationBuilder app)
    {
        app.Services.AddProblemDetails(config =>
        {
            config.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
                ctx.ProblemDetails.Extensions.Add("request-id", ctx.HttpContext.TraceIdentifier);
                ctx.ProblemDetails.Extensions.Add("correlation-id", ctx.HttpContext.Request.Headers["X-Correlation-ID"]);
                ctx.ProblemDetails.Extensions.Add("timestamp", DateTime.UtcNow);
            };
        });

        app.Services.AddExceptionHandler<ValidationExceptionHandler>();
        app.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void AddBackgroundJobsServices(WebApplicationBuilder app)
    {
        app.Services.AddScoped<KeycloakRegisterUserJob>();
        app.Services.AddScoped<KeycloakUpdateUserEmailJob>();
    }
}
