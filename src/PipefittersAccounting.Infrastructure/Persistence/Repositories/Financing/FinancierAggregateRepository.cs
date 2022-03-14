#pragma warning disable CS8602
#pragma warning disable CS8603

using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing
{
    public class FinancierAggregateRepository : IFinancierAggregateRepository, IDisposable
    {
        private bool isDisposed;
        private readonly AppDbContext _dbContext;

        public FinancierAggregateRepository(AppDbContext ctx) => _dbContext = ctx;

        ~FinancierAggregateRepository() => Dispose(false);

        public async Task<Financier> GetByIdAsync(Guid id) => await _dbContext.Financiers.FindAsync(id);

        public async Task<bool> Exists(Guid id) => await _dbContext.Financiers.FindAsync(id) != null;

        public async Task AddAsync(Financier entity) => await _dbContext.Financiers.AddAsync(entity);

        public void Update(Financier entity) => _dbContext.Financiers.Update(entity);

        public void Delete(Financier entity)
        {
            _dbContext.Financiers.Remove(entity);
            _dbContext.ExternalAgents.Remove(entity.ExternalAgent);
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateFinancierName(string name)
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from fin in _dbContext.Financiers.Where(e => String.Equals(e.FinancierName, name))
                                                                      .AsNoTracking()
                                     select new { FinancierId = fin.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.FinancierId;
                }

                return OperationResult<Guid>.CreateSuccessResult(returnValue);
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex);
            }
        }

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