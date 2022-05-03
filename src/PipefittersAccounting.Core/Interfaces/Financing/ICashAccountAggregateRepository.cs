using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashAccountAggregateRepository : IAggregateRootRepository
    {
        Task<OperationResult<CashAccount>> GetCashAccountByIdAsync(Guid cashAccountId);
        Task<OperationResult<bool>> DoesCashAccountExist(Guid cashAccountId);
        Task<OperationResult<bool>> AddCashAccountAsync(CashAccount cashAccount);
        Task<OperationResult<bool>> UpdateCashAccount(CashAccount cashAccount);
        Task<OperationResult<bool>> DeleteCashAccount(Guid cashAccountId);
    }
}

