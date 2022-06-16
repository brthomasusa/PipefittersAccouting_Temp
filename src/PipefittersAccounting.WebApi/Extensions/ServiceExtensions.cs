using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Commands.Identity;
using PipefittersAccounting.Infrastructure.Application.EventHandlers.Financing;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;

using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.WebApi.Interfaces.HumanResources;
using PipefittersAccounting.WebApi.Controllers.QueryHandlers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterDomainEventHandlers(this IServiceCollection services)
        {
            var serviceProvider = services
                .Scan(scan => scan
                    .FromAssemblyOf<CreateCashAccountEventHandler>()
                    .AddClasses(classes =>
                        classes.AssignableTo(typeof(IDomainEventHandler<>)))
                    .AsImplementedInterfaces()
                ).BuildServiceProvider();

            DomainEvent._serviceProvider = serviceProvider;

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services
                .AddScoped<AuthenticationCommandHandler>()
                .AddScoped<IUnitOfWork, AppUnitOfWork>()
                .AddScoped<IEmployeeAggregateQueryHandler, EmployeeAggregateQueryHandler>();
        }

        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IEmployeeAggregateRepository, EmployeeAggregateRepository>()
                .AddScoped<IFinancierAggregateRepository, FinancierAggregateRepository>()
                .AddScoped<ILoanAgreementAggregateRepository, LoanAgreementAggregateRepository>()
                .AddScoped<ICashAccountAggregateRepository, CashAccountAggregateRepository>()
                .AddScoped<IStockSubscriptionAggregateRepository, StockSubscriptionAggregateRepository>();
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IEmployeeAggregateApplicationService, EmployeeAggregateApplicationService>()
                .AddScoped<IFinancierApplicationService, FinancierApplicationService>()
                .AddScoped<ILoanAgreementApplicationService, LoanAgreementApplicationService>()
                .AddScoped<ICashAccountApplicationService, CashAccountApplicationService>()
                .AddScoped<IStockSubscriptionApplicationService, StockSubscriptionApplicationService>();
        }

        public static IServiceCollection AddDapperQueryServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IEmployeeAggregateQueryService, EmployeeAggregateQueryService>()
                .AddScoped<IFinancierQueryService, FinancierQueryService>()
                .AddScoped<ILoanAgreementQueryService, LoanAgreementQueryService>()
                .AddScoped<ICashAccountQueryService, CashAccountQueryService>()
                .AddScoped<IStockSubscriptionQueryService, StockSubscriptionQueryService>()
                .AddScoped<ISharedQueryService, SharedQueryService>();
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICashAccountAggregateValidationService, CashAccountAggregateValidationService>()
                .AddScoped<IStockSubscriptionValidationService, StockSubscriptionValidationService>()
                .AddScoped<ILoanAgreementValidationService, LoanAgreementValidationService>()
                .AddScoped<IEmployeeAggregateValidationService, EmployeeAggregateValidationService>()
                .AddScoped<IQueryServicesRegistry, QueryServicesRegistry>();
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
