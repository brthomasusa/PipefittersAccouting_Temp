using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IStockSubscriptionApplicationService
    {
        Task<OperationResult<bool>> CreateStockSubscription(StockSubscriptionWriteModel writeModel);
        Task<OperationResult<bool>> EditStockSubscription(StockSubscriptionWriteModel writeModel);
        Task<OperationResult<bool>> DeleteStockSubscription(StockSubscriptionWriteModel writeModel);
        Task<OperationResult<bool>> CreateDividendDeclaration(DividendDeclarationWriteModel writeModel);
        Task<OperationResult<bool>> EditDividendDeclaration(DividendDeclarationWriteModel writeModel);
        Task<OperationResult<bool>> DeleteDividendDeclaration(DividendDeclarationWriteModel writeModel);
    }
}