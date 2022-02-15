using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using TestSupport.EfHelpers;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.Sqlite
{
    public class SqliteDbContextFactory : IDisposable
    {
        private bool disposedValue = false;

        public AppDbContext CreateInMemoryContext()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            AppDbContext context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            return context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}