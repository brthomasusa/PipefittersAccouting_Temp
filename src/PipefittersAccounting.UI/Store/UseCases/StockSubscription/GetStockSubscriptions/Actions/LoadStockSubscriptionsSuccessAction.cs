using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions
{
    public class LoadStockSubscriptionsSuccessAction
    {
        public LoadStockSubscriptionsSuccessAction(PagingResponse<StockSubscriptionListItem> subscriptions)
        {
            CurrentSubscriptions = subscriptions;
        }

        public PagingResponse<StockSubscriptionListItem>? CurrentSubscriptions { get; }
    }
}