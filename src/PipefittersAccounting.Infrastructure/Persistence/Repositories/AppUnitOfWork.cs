using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories
{
    public class AppUnitOfWork : IUnitOfWork, IDisposable
    {
        private bool isDisposed;
        private readonly AppDbContext _dbContext;

        public AppUnitOfWork(AppDbContext ctx) => _dbContext = ctx;

        ~AppUnitOfWork() => Dispose(false);

        public async Task Commit() => await _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                // free managed resources
                _dbContext.Dispose();
            }

            isDisposed = true;
        }
    }
}