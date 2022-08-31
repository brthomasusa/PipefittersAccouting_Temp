using Fluxor;
using PipefittersAccounting.UI.Store.State;
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
            => new StockSubscriptionState(true, null, null, state.CurrentSubscription);


        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionSuccessAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionSuccessAction action
        )
            => new StockSubscriptionState(false, null, action.CurrentSubscriptions, state.CurrentSubscription);

        [ReducerMethod]
        public static StockSubscriptionState ReduceLoadStockSubscriptionFailureAction
        (
            StockSubscriptionState state,
            LoadStockSubscriptionFailureAction action
        )
            => new StockSubscriptionState(false, action.ErrorMessage, null, state.CurrentSubscription);
    }
}