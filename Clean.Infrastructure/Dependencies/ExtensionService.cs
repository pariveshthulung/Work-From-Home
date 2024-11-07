using Clean.Application.Persistence.Contract;
using Clean.Application.Persistence.Services;
using Clean.Infrastructure.Repository;
using Clean.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Infrastructure.Dependencies
{
    public static class ExtensionService
    {
        public static void AddInfrastructureConfig(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            // services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEnumRepository, EnumRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
