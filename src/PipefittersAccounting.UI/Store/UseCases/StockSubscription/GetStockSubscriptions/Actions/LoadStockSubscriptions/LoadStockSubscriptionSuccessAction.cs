using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions.LoadStockSubscriptions
{
    public class LoadStockSubscriptionSuccessAction
    {
        public LoadStockSubscriptionSuccessAction(PagingResponse<StockSubscriptionListItem> subscriptions) =>
            CurrentSubscriptions = subscriptions;

        public PagingResponse<StockSubscriptionListItem>? CurrentSubscriptions { get; }
    }
}