using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface IStockSubscriptionAggregateRepository
    {
        Task<OperationResult<StockSubscription>> GetStockSubscriptionByIdAsync(Guid stockId);
        Task<OperationResult<DividendDeclaration>> GetDividendDeclarationByIdAsync(Guid dividendId);
        Task<OperationResult<bool>> DoesStockSubscriptionExist(Guid stockId);
        Task<OperationResult<bool>> AddStockSubscriptionAsync(StockSubscription subscription);
        OperationResult<bool> UpdateStockSubscription(StockSubscription subscription);
        Task<OperationResult<bool>> DeleteStockSubscriptionAsync(Guid stockId);
        Task<OperationResult<bool>> DeleteDividendDeclarationAsync(Guid dividendId);
    }
}
