using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountApplicationService
    {
        Task<OperationResult<bool>> CreateCashAccount(CashAccountWriteModel writeModel);
        Task<OperationResult<bool>> UpdateCashAccount(CashAccountWriteModel writeModel);
        Task<OperationResult<bool>> DeleteCashAccount(CashAccountWriteModel writeModel);
        Task<OperationResult<bool>> CreateCashDeposit(CashTransactionWriteModel writeModel);
        Task<OperationResult<bool>> CreateCashDisbursement(CashTransactionWriteModel writeModel);
        Task<OperationResult<bool>> CreateCashTransfer(CashAccountTransferWriteModel writeModel);
        Task<OperationResult<bool>> CreateDisbursementsForPayroll(Guid cashAccountId, Guid userId);
    }
}