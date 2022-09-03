using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.CreateStockSubscription.Actions
{
    public class CreateStockSubscriptionAction
    {
        public CreateStockSubscriptionAction(StockSubscriptionWriteModel subscription)
            => StockSubscription = subscription;

        public StockSubscriptionWriteModel StockSubscription { get; init; }
    }
}