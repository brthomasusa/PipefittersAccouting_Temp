#pragma warning disable CS8600

using Microsoft.EntityFrameworkCore;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing
{
    public class CashAccountAggregateRepository : ICashAccountAggregateRepository
    {
        private bool _isDisposed;
        private readonly AppDbContext _dbContext;

        public CashAccountAggregateRepository(AppDbContext ctx) => _dbContext = ctx;
        ~CashAccountAggregateRepository() => Dispose(false);

        public async Task<OperationResult<CashAccount>> GetCashAccountByIdAsync(Guid id)
        {
            try
            {
                CashAccount cashAccount = await _dbContext.CashAccounts.FindAsync(id);

                if (cashAccount is null)
                {
                    string msg = $"Unable to locate a cash account with id '{id}'.";
                    return OperationResult<CashAccount>.CreateFailure(msg);
                }

                return OperationResult<CashAccount>.CreateSuccessResult(cashAccount);
            }
            catch (Exception ex)
            {
                return OperationResult<CashAccount>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> DoesCashAccountExist(Guid id)
        {
            try
            {
                bool exist = await _dbContext.CashAccounts.FindAsync(id) != null;

                if (exist)
                {
                    return OperationResult<bool>.CreateSuccessResult(exist);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure($"A cash account with id '{id}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddCashAccountAsync(CashAccount entity)
        {
            try
            {
                OperationResult<Guid> dupeNameResult = await CheckForDuplicateAccountName(entity.CashAccountName);
                if (dupeNameResult.Result != Guid.Empty)
                {
                    string errMsg = $"A cash account with account name: {entity.CashAccountName} is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeAcctNumberResult = await CheckForDuplicateAccountNumber(entity.CashAccountNumber);
                if (dupeAcctNumberResult.Result != Guid.Empty)
                {
                    string msg = $"A cash account with account number: {entity.CashAccountNumber} is already in the database.";
                    return OperationResult<bool>.CreateFailure(msg);
                }

                await _dbContext.CashAccounts.AddAsync(entity);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> UpdateCashAccount(CashAccount entity)
        {
            try
            {
                _dbContext.CashAccounts.Update(entity);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> DeleteCashAccount(CashAccount entity)
        {
            try
            {
                _dbContext.CashAccounts.Remove(entity);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateAccountName(string name)
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from cashAcct in _dbContext.CashAccounts.Where(e => String.Equals(e.CashAccountName, name))
                                                                             .AsNoTracking()
                                     select new { CashAccountId = cashAcct.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.CashAccountId;
                }

                return OperationResult<Guid>.CreateSuccessResult(returnValue);
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex);
            }
        }

        public async Task<OperationResult<Guid>> CheckForDuplicateAccountNumber(string acctNumber)
        {
            try
            {
                Guid returnValue = Guid.Empty;

                var details = await (from cashAcct in _dbContext.CashAccounts.Where(e => String.Equals(e.CashAccountNumber, acctNumber))
                                                                             .AsNoTracking()
                                     select new { CashAccountId = cashAcct.Id }).FirstOrDefaultAsync();

                if (details is not null)
                {
                    returnValue = details.CashAccountId;
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