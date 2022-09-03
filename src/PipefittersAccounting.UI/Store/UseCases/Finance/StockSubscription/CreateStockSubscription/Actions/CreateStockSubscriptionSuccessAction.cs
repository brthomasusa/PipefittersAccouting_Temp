using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.CreateStockSubscription.Actions
{
    public class CreateStockSubscriptionSuccessAction
    {
        public CreateStockSubscriptionSuccessAction(StockSubscriptionWriteModel subscription)
            => StockSubscription = subscription;

        public StockSubscriptionWriteModel StockSubscription { get; init; }
    }
}