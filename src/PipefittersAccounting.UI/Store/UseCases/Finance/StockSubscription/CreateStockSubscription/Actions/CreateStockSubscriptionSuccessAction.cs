using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions
{
    public class CreateStockSubscriptionSuccessAction
    {
        public CreateStockSubscriptionSuccessAction
        (
            StockSubscriptionWriteModel subscription
        )
        {
            StockSubscriptionCreateModel = subscription;
        }

        public StockSubscriptionWriteModel StockSubscriptionCreateModel { get; init; }
    }
}