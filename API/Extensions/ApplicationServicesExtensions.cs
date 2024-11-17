using API.Middleware;
using API.Services;
using Application.Core;
using Application.Equipments.Command.CreateEquipment;
using Application.Equipments.Queries.GetEquipments;
using Application.Job.PaymentStatusJob;
using Application.Job.SparepartStatusJob;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Quartz;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Heavy-Equipment", Version = "v1" });

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Enter your valid token in the text input below.\n\nExample: '12345abcdef'"
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );
            });
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy(
                    "CorsPolicy",
                    policy =>
                    {
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins(
                                "http://localhost:3000",
                                "http://localhost:8081",
                                "https://103.127.137.194:3000",
                                "https://heavy-equipment.my.id",
                                "http://heavy-equipment.my.id",
                                "https://www.heavy-equipment.my.id"
                            );
                    }
                );
            });
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetEquipmentQuery.Handler).Assembly)
            );
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateEquipmentCommand>();
            services.AddSignalR();
            services.AddScoped<EmailServices>();
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("PaymentStatusUpdateJob");
                q.AddJob<PaymentStatusJobs>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts =>
                    opts.ForJob(jobKey)
                        .WithIdentity("PaymentStatusUpdateTrigger")
                        .WithCronSchedule("0 0/1 * * * ?")
                );

                var sparepartJobKey = new JobKey("SparepartStatusUpdateJob");
                q.AddJob<SparepartStatusJobs>(opts => opts.WithIdentity(sparepartJobKey));
                q.AddTrigger(opts =>
                    opts.ForJob(sparepartJobKey)
                        .WithIdentity("SparepartStatusUpdateTrigger")
                        .WithCronSchedule("0 0/1 * * * ?")
                );
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            return services;
        }
    }
}
