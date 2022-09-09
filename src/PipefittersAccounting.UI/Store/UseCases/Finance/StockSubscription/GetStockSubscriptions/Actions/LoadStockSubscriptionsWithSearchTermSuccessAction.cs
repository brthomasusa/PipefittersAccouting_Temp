using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions
{
    public class LoadStockSubscriptionsWithSearchTermSuccessAction
    {
        public LoadStockSubscriptionsWithSearchTermSuccessAction(PagingResponse<StockSubscriptionListItem> subscriptions)
        {
            CurrentSubscriptions = subscriptions;
        }

        public PagingResponse<StockSubscriptionListItem>? CurrentSubscriptions { get; }
    }
}