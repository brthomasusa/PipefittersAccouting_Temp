using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IFinancierCommandService
    {
        Task<OperationResult<bool>> CreateFinancierInfo(CreateFinancierInfo writeModel);
        Task<OperationResult<bool>> EditFinancierInfo(EditFinancierInfo writeModel);
        Task<OperationResult<bool>> DeleteFinancierInfo(DeleteFinancierInfo writeModel);
    }
}