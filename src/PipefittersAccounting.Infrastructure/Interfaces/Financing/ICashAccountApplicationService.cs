using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountApplicationService
    {
        Task<OperationResult<bool>> CreateCashAccount(CreateCashAccountInfo writeModel);
        Task<OperationResult<bool>> UpdateCashAccount(EditCashAccountInfo writeModel);
        Task<OperationResult<bool>> DeleteCashAccount(DeleteCashAccountInfo writeModel);
        Task<OperationResult<bool>> CreateCashDeposit(CreateCashAccountTransactionInfo writeModel);
        Task<OperationResult<bool>> CreateCashDisbursement(CreateCashAccountTransactionInfo writeModel);
        Task<OperationResult<bool>> CreateCashTransfer(CreateCashAccountTransferInfo writeModel);
    }
}