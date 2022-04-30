#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604

using Microsoft.EntityFrameworkCore;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing
{
    public class CashAccountAggregateRepository : ICashAccountAggregateRepository
    {
        private bool _isDisposed;
        private readonly AppDbContext _dbContext;

        public CashAccountAggregateRepository(AppDbContext ctx) => _dbContext = ctx;
        ~CashAccountAggregateRepository() => Dispose(false);

        public Task<OperationResult<CashAccount>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> Exists(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> AddAsync(CashAccount entity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> Update(CashAccount entity)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> Delete(CashAccount entity)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // free managed resources
                _dbContext.Dispose();
            }

            _isDisposed = true;
        }
    }
}