using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
            // cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly)
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()) //try
        );
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoginPipelineBehaviour<,>));
        return services;
    }
}
