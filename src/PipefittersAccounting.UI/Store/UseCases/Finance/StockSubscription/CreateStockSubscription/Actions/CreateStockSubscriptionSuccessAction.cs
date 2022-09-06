using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions
{
    public class CreateStockSubscriptionSuccessAction
    {
        public CreateStockSubscriptionSuccessAction
        (
            PagingResponse<StockSubscriptionListItem> subscriptionList,
            StockSubscriptionReadModel subscription
        )
        {
            StockSubscriptionList = subscriptionList;
            StockSubscriptionReadModel = subscription;
        }

        public PagingResponse<StockSubscriptionListItem>? StockSubscriptionList { get; init; }

        public StockSubscriptionReadModel? StockSubscriptionReadModel { get; init; }
    }
}