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
        protected readonly string _connectionString;
        protected readonly AppDbContext _dbContext;
        protected readonly DapperContext _dapperCtx;
        protected readonly string serviceAddress = "https://localhost:7035/";

        public TestBaseEfCore()
        {
            var config = AppSettings.GetConfiguration();
            _connectionString = config.GetConnectionString(_defaultConnectionString);
            _dapperCtx = new DapperContext(_connectionString);

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                _connectionString,
                msSqlOptions => msSqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseLazyLoadingProxies();

            _dbContext = new AppDbContext(optionsBuilder.Options);
            _dbContext.Database.ExecuteSqlRaw("EXEC dbo.usp_resetTestDb");  // This is 4 times as fast (897ms -vs- 4 seconds) as calling
            // TestDatabaseInitialization.InitializeData(_dbContext);       // TestDatabaseInitialization.InitializeData(_dbContext)
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
