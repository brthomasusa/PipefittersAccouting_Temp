using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IFinancierApplicationService
    {
        Task<OperationResult<bool>> CreateFinancierInfo(FinancierWriteModel writeModel);
        Task<OperationResult<bool>> EditFinancierInfo(FinancierWriteModel writeModel);
        Task<OperationResult<bool>> DeleteFinancierInfo(FinancierWriteModel writeModel);
    }
}