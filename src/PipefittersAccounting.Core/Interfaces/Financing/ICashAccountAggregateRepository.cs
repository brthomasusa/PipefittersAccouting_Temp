using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public delegate Task<OperationResult<bool>> DoTransactionDelegate();

    public interface ICashAccountAggregateRepository : IAggregateRootRepository<CashAccount>
    {
        Task<OperationResult<bool>> AddCashTransferAsync(CashTransfer cashTransfer);
        Task<OperationResult<bool>> UpdateCashAccountAsync(CashAccount cashAccount);
        Task<OperationResult<bool>> DeleteCashAccountAsync(Guid cashAccountId);
        Task<OperationResult<bool>> ExecuteInATransaction(DoTransactionDelegate funcToExecute);
    }
}

