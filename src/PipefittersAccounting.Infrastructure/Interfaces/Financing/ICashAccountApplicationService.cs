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
        Task<OperationResult<bool>> CreateCashDeposit(CreateCashAccountTransactionInfo writeModel);
        Task<OperationResult<bool>> CreateCashDisbursement(CreateCashAccountTransactionInfo writeModel);
        Task<OperationResult<bool>> CreateCashTransfer(CashAccountTransferWriteModel writeModel);
    }
}