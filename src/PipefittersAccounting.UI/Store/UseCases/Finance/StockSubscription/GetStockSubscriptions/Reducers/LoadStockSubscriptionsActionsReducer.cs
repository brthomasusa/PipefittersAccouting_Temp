using Fluxor;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Reducers
{
    public static class LoadStockSubscriptionsActionsReducer
    {
        [ReducerMethod]
        public static StockSubscriptionsState ReduceLoadStockSubscriptionsAction
        (
            StockSubscriptionsState state,
            LoadStockSubscriptionsAction _
        )
        {
            return new StockSubscriptionsState
                (
                    true,
                    null,
                    null,
                    state.StockSubscriptionReadModel,
                    state.PageNumber,
                    state.PageSize,
                    state.CreatePagePath,
                    state.SubscriptionListFilter
                );
        }

        [ReducerMethod]
        public static StockSubscriptionsState ReduceLoadStockSubscriptionsSuccessAction
        (
            StockSubscriptionsState state,
            LoadStockSubscriptionsSuccessAction action
        )
        {
            return new StockSubscriptionsState
                (
                    false,
                    null,
                    action.CurrentSubscriptions,
                    state.StockSubscriptionReadModel,
                    action.CurrentSubscriptions!.MetaData!.CurrentPage,
                    action.CurrentSubscriptions!.MetaData!.PageSize,
                    state.CreatePagePath,
                    action.SubscriptionListFilter
                );
        }

        [ReducerMethod]
        public static StockSubscriptionsState ReduceLoadStockSubscriptionsWithSearchTermSuccessAction
        (
            StockSubscriptionsState state,
            LoadStockSubscriptionsWithSearchTermSuccessAction action
        )
        {
            return new StockSubscriptionsState
                (
                    false,
                    null,
                    action.CurrentSubscriptions,
                    state.StockSubscriptionReadModel,
                    action.CurrentSubscriptions!.MetaData!.CurrentPage,
                    action.CurrentSubscriptions!.MetaData!.PageSize,
                    state.CreatePagePath,
                    state.SubscriptionListFilter
                );
        }

        [ReducerMethod]
        public static StockSubscriptionsState ReduceLoadStockSubscriptionFailureAction
        (
            StockSubscriptionsState state,
            LoadStockSubscriptionsFailureAction action
        )
        {
            return new StockSubscriptionsState
                (
                    false,
                    action.ErrorMessage,
                    null,
                    state.StockSubscriptionReadModel,
                    state.PageNumber,
                    state.PageSize,
                    state.CreatePagePath,
                    state.SubscriptionListFilter
                );
        }
    }
}