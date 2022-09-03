using Fluxor;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Reducers
{
    public static class LoadStockSubscriptionsActionsReducer
    {
        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionsUnfilteredAction _
        )
        {
            return new StockSubscriptionState(true, null, null, state.CurrentSubscription);
        }


        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionSuccessAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionsSuccessAction action
        )
        {
            return new StockSubscriptionState(false, null, action.CurrentSubscriptions, state.CurrentSubscription);
        }

        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionFailureAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionsFailureAction action
        )
        {
            return new StockSubscriptionState(false, action.ErrorMessage, null, state.CurrentSubscription);
        }
    }
}