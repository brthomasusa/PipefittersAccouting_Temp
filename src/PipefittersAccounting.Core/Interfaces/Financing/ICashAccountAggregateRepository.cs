using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashAccountAggregateRepository : IAggregateRootRepository
    {
        Task<OperationResult<CashAccount>> GetCashAccountByIdAsync(Guid id);
        Task<OperationResult<bool>> DoesCashAccountExist(Guid id);
        Task<OperationResult<bool>> AddCashAccountAsync(CashAccount entity);
        OperationResult<bool> UpdateCashAccount(CashAccount entity);
        OperationResult<bool> DeleteCashAccount(CashAccount entity);
        Task<OperationResult<Guid>> CheckForDuplicateAccountName(string name);
        Task<OperationResult<Guid>> CheckForDuplicateAccountNumber(string name);
    }
}

