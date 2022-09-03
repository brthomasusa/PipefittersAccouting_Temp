using PipefittersAccounting.UI.Store.UseCases.Shared.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions
{
    public class LoadStockSubscriptionsFailureAction : FailureAction
    {
        public LoadStockSubscriptionsFailureAction(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}