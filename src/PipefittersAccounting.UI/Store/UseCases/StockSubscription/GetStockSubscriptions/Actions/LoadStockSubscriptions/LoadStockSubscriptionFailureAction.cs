using PipefittersAccounting.UI.Store.UseCases.Shared.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions.LoadStockSubscriptions
{
    public class LoadStockSubscriptionFailureAction : FailureAction
    {
        public LoadStockSubscriptionFailureAction(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}