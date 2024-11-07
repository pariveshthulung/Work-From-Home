using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Domain;

public static class ServiceRegistration
{
    public static IServiceCollection AddDomainLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            // cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly)
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()) //ttry
        );
        return services;
    }
}
