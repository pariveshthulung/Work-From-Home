using System.Net.Mail;
using System.Text;
using Clean.Api.Settings;
using Clean.Domain.Entities;
using Clean.Infrastructure.Data;
using Clean.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Clean.Api.Dependency
{
    public static class ServiceRegistration
    {
        static JwtSettings? JwtSettings { get; set; } = new();
        static SmtpSettings? SmtpSettings { get; set; } = new();
        static ConnectionSettings? ConnectionStrings { get; set; } = new();
        static OktaSettings? OktaSettings { get; set; } = new();

        public static void AddPresentationLayer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.JsonConfigure(configuration);
            services.DatabaseConfiguration(configuration);
            services.SwaggerConfiguration(configuration);
            services.IdentityConfiguration();
            services.JwtConfiguration(configuration);
            services.NewtonsoftConfiguration();
            services.AuthorizationConfiguration();
            services.EmailConfigure();
        }

        public static void JsonConfigure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            ConnectionStrings = configuration
                .GetSection("ConnectionStrings")
                .Get<ConnectionSettings>();
            JwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
            SmtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            OktaSettings = configuration.GetSection("Okta").Get<OktaSettings>();
        }

        public static void EmailConfigure(this IServiceCollection services)
        {
            Console.WriteLine($"FromEmail: {SmtpSettings?.FromEmail}, Host: {SmtpSettings?.Host}");

            services
                .AddFluentEmail(SmtpSettings?.FromEmail, SmtpSettings?.FromName)
                .AddRazorRenderer()
                .AddSmtpSender(
                    new SmtpClient(SmtpSettings?.Host)
                    {
                        Port = SmtpSettings!.Port,
                        Credentials = new System.Net.NetworkCredential(
                            SmtpSettings.UserName,
                            SmtpSettings.Password
                        ),
                        EnableSsl = true,
                    }
                );
        }

        public static void IdentityConfiguration(this IServiceCollection services)
        {
            services
                .AddIdentity<AppUser, UserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void SwaggerConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkFromHome", Version = "v1" });

                c.AddSecurityDefinition(
                    "oauth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(
                                    // $"{configuration["Okta:OktaDomain"]}/oauth2/default/v1/authorize"
                                    $"{OktaSettings?.OktaDomain}/oauth2/ausjszru5jA7nhqRh5d7/v1/authorize"
                                ),
                                TokenUrl = new Uri(
                                    // $"{configuration["Okta:OktaDomain"]}/oauth2/default/v1/token"
                                    $"{OktaSettings?.OktaDomain}/oauth2/ausjszru5jA7nhqRh5d7/v1/token"
                                ),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "openid", "OpenID Connect" },
                                    { "profile", "Profile Information" },
                                    { "email", "Email" }
                                }
                            }
                        }
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
                                    Id = "oauth2"
                                }
                            },
                            new[] { "openid", "profile", "email" }
                        }
                    }
                );
            });
        }

        public static void DatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddSingleton<ConvertDomainEventIntoOutboxMessageInterceptor>();
            services.AddDbContext<ApplicationDbContext>(
                (sp, options) =>
                {
                    var interceptor =
                        sp.GetService<ConvertDomainEventIntoOutboxMessageInterceptor>();
                    options
                        .UseSqlServer(ConnectionStrings?.DefaultConnection)
                        .AddInterceptors(interceptor!);
                    // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.EnableSensitiveDataLogging();
                }
            );
        }

        public static void JwtConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    "okta",
                    option =>
                    {
                        option.Authority =
                            $"{OktaSettings?.OktaDomain}/oauth2/ausjszru5jA7nhqRh5d7";
                        option.Audience = "swaggeroauth";
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = false,
                        };
                    }
                )
                .AddJwtBearer(
                    "custom",
                    option =>
                    {
                        option.Authority =
                            $"{OktaSettings?.OktaDomain}/oauth2/ausjszru5jA7nhqRh5d7";
                        option.Audience = "swaggeroauth";
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(JwtSettings?.SigningKey ?? string.Empty)
                            )
                        };
                    }
                );
            ;
        }

        public static void AuthorizationConfiguration(this IServiceCollection services)
        {
            services.AddAuthorization();

            services
                .AddAuthorizationBuilder()
                .AddPolicy(
                    "Adminstrator",
                    policy => policy.RequireRole("Ceo", "Manager", "SuperAdmin", "Admin")
                )
                .AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"))
                .AddPolicy(
                    "AllEmployee",
                    policy =>
                        policy.RequireRole(
                            "Ceo",
                            "Manager",
                            "Intern",
                            "Developer",
                            "SuperAdmin",
                            "Admin"
                        )
                );
        }

        public static void NewtonsoftConfiguration(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                        .Json
                        .ReferenceLoopHandling
                        .Ignore;
                });
        }
    }
}
