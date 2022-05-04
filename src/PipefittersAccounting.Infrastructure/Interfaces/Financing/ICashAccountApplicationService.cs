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
    }
}