using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IStockSubscriptionQueryService
    {
        Task<OperationResult<StockSubscriptionDetails>> GetStockSubscriptionDetails(GetStockSubscriptionParameters queryParameters);
        Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetCashAccountListItems(GetStockSubscriptionListItemParameters queryParameters);
    }
}