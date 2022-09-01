using Fluxor;
using PipefittersAccounting.UI.Store.State.StockSubscription;
using PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions.LoadStockSubscriptions;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Reducers
{
    public static class LoadStockSubscriptionsActionsReducer
    {
        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionAction _
        )
        {
            Console.WriteLine("LoadStockSubscriptionsActionsReducer.ReduceLoadStockSubscriptionAction called...");
            return new StockSubscriptionState(true, null, null, state.CurrentSubscription);
        }


        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionSuccessAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionSuccessAction action
        )
        {
            Console.WriteLine("LoadStockSubscriptionsActionsReducer.ReduceLoadStockSubscriptionSuccessAction called...");
            return new StockSubscriptionState(false, null, action.CurrentSubscriptions, state.CurrentSubscription);
        }

        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionFailureAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionFailureAction action
        )
        {
            Console.WriteLine("LoadStockSubscriptionsActionsReducer.ReduceLoadStockSubscriptionFailureAction called...");
            return new StockSubscriptionState(false, action.ErrorMessage, null, state.CurrentSubscription);
        }
    }
}