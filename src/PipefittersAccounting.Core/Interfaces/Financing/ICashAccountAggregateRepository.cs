using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public delegate Task<OperationResult<bool>> DoTransactionDelegate();

    public interface ICashAccountAggregateRepository : IAggregateRootRepository
    {
        Task<OperationResult<CashAccount>> GetCashAccountByIdAsync(Guid cashAccountId);
        Task<OperationResult<bool>> DoesCashAccountExist(Guid cashAccountId);
        Task<OperationResult<bool>> AddCashAccountAsync(CashAccount cashAccount);
        Task<OperationResult<bool>> AddCashTransferAsync(CashTransfer cashTransfer);
        Task<OperationResult<bool>> UpdateCashAccountAsync(CashAccount cashAccount);
        Task<OperationResult<bool>> DeleteCashAccountAsync(Guid cashAccountId);
        Task<OperationResult<bool>> ExecuteInATransaction(DoTransactionDelegate funcToExecute);
    }
}

