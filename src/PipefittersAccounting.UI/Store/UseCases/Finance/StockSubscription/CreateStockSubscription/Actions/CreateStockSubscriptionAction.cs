using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions
{
    public class CreateStockSubscriptionAction
    {
        public CreateStockSubscriptionAction(StockSubscriptionWriteModel subscription)
            => StockSubscriptionWriteModel = subscription;

        public StockSubscriptionWriteModel StockSubscriptionWriteModel { get; init; }
    }
}