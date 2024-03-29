#pragma warning disable CS8600

using Microsoft.EntityFrameworkCore;

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Persistence.Repositories.CashManagement
{


    public class CashAccountAggregateRepository : ICashAccountAggregateRepository
    {
        private bool _isDisposed;
        private readonly AppDbContext _dbContext;

        public CashAccountAggregateRepository(AppDbContext ctx) => _dbContext = ctx;
        ~CashAccountAggregateRepository() => Dispose(false);

        public async Task<OperationResult<bool>> ExecuteInATransaction(DoTransactionDelegate funcToExecute)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                await funcToExecute();
                await transaction.CommitAsync();
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<CashAccount>> GetByIdAsync(Guid cashAccountId)
        {
            try
            {
                CashAccount? cashAccount = await _dbContext.CashAccounts.Where(acct => acct.Id == cashAccountId)
                                                                        .Include(trans => trans.CashTransactions.Where(a => a.CashAccountId == cashAccountId))
                                                                        .FirstOrDefaultAsync();

                if (cashAccount is null)
                {
                    string msg = $"Unable to locate a cash account with id '{cashAccountId}'.";
                    return OperationResult<CashAccount>.CreateFailure(msg);
                }

                return OperationResult<CashAccount>.CreateSuccessResult(cashAccount);
            }
            catch (Exception ex)
            {
                return OperationResult<CashAccount>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> Exists(Guid cashAccountId)
        {
            try
            {
                bool exist = await _dbContext.CashAccounts.FindAsync(cashAccountId) != null;

                if (exist)
                {
                    return OperationResult<bool>.CreateSuccessResult(exist);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure($"A cash account with id '{cashAccountId}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddAsync(CashAccount cashAccount)
        {
            try
            {
                OperationResult<bool> searchResult = await Exists(cashAccount.Id);

                if (searchResult.Result)
                {
                    string errMsg = $"A cash account with id '{cashAccount.Id}' is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeNameResult = await CheckForDuplicateAccountName(cashAccount.CashAccountName);

                if (dupeNameResult.Result != Guid.Empty)
                {
                    string errMsg = $"A cash account with account name '{cashAccount.CashAccountName}' is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeAcctNumberResult = await CheckForDuplicateAccountNumber(cashAccount.CashAccountNumber);

                if (dupeAcctNumberResult.Result != Guid.Empty)
                {
                    string msg = $"A cash account with account number '{cashAccount.CashAccountNumber}' is already in the database.";
                    return OperationResult<bool>.CreateFailure(msg);
                }

                EconomicResource economicResource = new(EntityGuidID.Create(cashAccount.Id), ResourceTypeEnum.Cash);
                await _dbContext.EconomicResources.AddAsync(economicResource);
                await _dbContext.CashAccounts.AddAsync(cashAccount);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> AddCashTransferAsync(CashTransfer cashTransfer)
        {
            try
            {
                EconomicEvent evt = new(EntityGuidID.Create(cashTransfer.Id), EventTypeEnum.CashTransfered);
                await _dbContext.EconomicEvents.AddAsync(evt);
                await _dbContext.CashAccountTransfers.AddAsync(cashTransfer);

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        // This is just to satisfy interface requirements, update is actually done by UpdateCashAccountAsync()
        public OperationResult<bool> Update(CashAccount entity) => OperationResult<bool>.CreateSuccessResult(true);


        public async Task<OperationResult<bool>> UpdateCashAccountAsync(CashAccount cashAccount)
        {
            try
            {
                OperationResult<bool> searchResult = await Exists(cashAccount.Id);

                if (!searchResult.Result)
                {
                    string errMsg = $"Update failed! A cash account with id: {cashAccount.Id} could not be located.";
                    return OperationResult<bool>.CreateFailure(errMsg); ;
                }

                OperationResult<Guid> dupeNameResult = await CheckForDuplicateAccountName(cashAccount.CashAccountName);

                if (dupeNameResult.Result != Guid.Empty && dupeNameResult.Result != cashAccount.Id)
                {
                    string errMsg = $"A cash account with account name: {cashAccount.CashAccountName} is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeAcctNumberResult = await CheckForDuplicateAccountNumber(cashAccount.CashAccountNumber);

                if (dupeAcctNumberResult.Result != Guid.Empty && dupeAcctNumberResult.Result != cashAccount.Id)
                {
                    string msg = $"A cash account with account number: {cashAccount.CashAccountNumber} is already in the database.";
                    return OperationResult<bool>.CreateFailure(msg);
                }

                _dbContext.CashAccounts.Update(cashAccount);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        // This is just to satisfy interface requirements, delete is actually done by DeleteCashAccountAsync()
        public OperationResult<bool> Delete(CashAccount entity) => OperationResult<bool>.CreateSuccessResult(true);

        public async Task<OperationResult<bool>> DeleteCashAccountAsync(Guid cashAccountId)
        {
            try
            {
                CashAccount cashAccount = await _dbContext.CashAccounts.FindAsync(cashAccountId);

                if (cashAccount is not null)
                {
                    EconomicResource resource = await _dbContext.EconomicResources.FindAsync(cashAccount.Id);

                    if (resource is not null)
                    {
                        _dbContext.CashAccounts.Remove(cashAccount);
                        _dbContext.EconomicResources.Remove(resource);
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        string msg = $"Delete cash account failed! Unable to locate EconomicResource associated with this account.";
                        return OperationResult<bool>.CreateFailure(msg);
                    }

                }
                else
                {
                    string errMsg = $"Delete failed! A cash account with id: {cashAccountId} could not be located.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        private async Task<OperationResult<Guid>> CheckForDuplicateAccountName(string name)
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

        private async Task<OperationResult<Guid>> CheckForDuplicateAccountNumber(string acctNumber)
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