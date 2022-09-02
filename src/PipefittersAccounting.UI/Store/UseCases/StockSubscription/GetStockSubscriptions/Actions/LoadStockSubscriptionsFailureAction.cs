using PipefittersAccounting.UI.Store.UseCases.Shared.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions
{
    public class LoadStockSubscriptionsFailureAction : FailureAction
    {
        public LoadStockSubscriptionsFailureAction(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}