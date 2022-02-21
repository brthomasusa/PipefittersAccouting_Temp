using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using TestSupport.Helpers;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore
{
    public abstract class TestBaseEfCore : IDisposable
    {
        private const string _defaultConnectionString = "DefaultConnection";
        private readonly string _connectionString;
        protected readonly AppDbContext _dbContext;
        protected readonly string serviceAddress = "https://localhost:7035/";

        public TestBaseEfCore()
        {
            var config = AppSettings.GetConfiguration();
            _connectionString = config.GetConnectionString(_defaultConnectionString);

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                _connectionString,
                msSqlOptions => msSqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseLazyLoadingProxies();

            _dbContext = new AppDbContext(optionsBuilder.Options);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}