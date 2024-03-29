using PipefittersAccounting.UI.Store.UseCases.Shared.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions
{
    public class CreateStockSubscriptionFailureAction : FailureAction
    {
        public CreateStockSubscriptionFailureAction(string errorMessage)
            : base(errorMessage)
        { }
    }
}