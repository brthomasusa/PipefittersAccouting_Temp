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
        Task<OperationResult<bool>> UpdateStockSubscription(StockSubscriptionWriteModel writeModel);
        Task<OperationResult<bool>> DeleteStockSubscription(StockSubscriptionWriteModel writeModel);
    }
}