using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using PipefittersAccounting.Infrastructure.Application.Commands.Identity;
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Queries.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;


using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services
                .AddScoped<AuthenticationCommandHandler>()
                .AddScoped<IUnitOfWork, AppUnitOfWork>()
                .AddScoped<IEmployeeAggregateRepository, EmployeeAggregateRepository>()
                .AddScoped<IEmployeeAggregateCommandHandler, EmployeeAggregateCommandHandler>()
                .AddScoped<IEmployeeAggregateCommandHandlerService, EmployeeCommandHandlerServiceEfCore>()
                .AddScoped<IEmployeeAggregateQueryHandler, EmployeeAggregateQueryHandler>()
                .AddScoped<IEmployeeAggregateQueryService, EmployeeAggregateQueryService>();
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Pagination"));
            });

        public static void ConfigureEfCoreDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration["ConnectionStrings:DefaultConnection"],
                    msSqlOptions => msSqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseLazyLoadingProxies()
            );
        }

        public static void ConfigureDapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DapperContext>(s => new DapperContext(configuration["ConnectionStrings:DefaultConnection"]));
        }

        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(opts =>
                opts.UseSqlServer(configuration["ConnectionStrings:IdentityConnection"]));

            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = configuration.GetSection("JwtApiKey").Value;

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}