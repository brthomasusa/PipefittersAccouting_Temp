using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions
{
    public class LoadStockSubscriptionsSuccessAction
    {
        public LoadStockSubscriptionsSuccessAction
        (
            PagingResponse<StockSubscriptionListItem> subscriptions,
            string filterName,
            int pageSize
        )
        {
            CurrentSubscriptions = subscriptions;
            SubscriptionListFilter = filterName;
        }

        public PagingResponse<StockSubscriptionListItem>? CurrentSubscriptions { get; }
        public string SubscriptionListFilter { get; }
        public int PageSize { get; }
    }
}