using Clean.Application.Persistence.Contract;
using Clean.Application.Persistence.Services;
using Clean.Infrastructure.Backgrounds;
using Clean.Infrastructure.Repository;
using Clean.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Clean.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        // services.AddQuartz();

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEnumRepository, EnumRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }

    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutBoxMessagesJob));
            configure
                .AddJob<ProcessOutBoxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(10).RepeatForever()
                        )
                );
        });
        services.AddQuartzHostedService();
        return services;
    }
}
