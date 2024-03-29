using Fluxor;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.CreateStockSubscription.Reducers
{
    public static class CreateStockSubscriptionActionReducer
    {
        [ReducerMethod]
        public static StockSubscriptionsState ReduceCreateStockSubscriptionAction
        (
            StockSubscriptionsState state,
            CreateStockSubscriptionAction _
        )
            => new StockSubscriptionsState
                (
                    true,
                    null,
                    state.StockSubscriptionList,
                    state.StockSubscriptionReadModel,
                    state.PageNumber,
                    state.PageSize,
                    state.CreatePagePath,
                    state.SubscriptionListFilter,
                    state.StockSubscriptionCreateModel,
                    state.StockSubscriptionEditModel
                );

        [ReducerMethod]
        public static StockSubscriptionsState ReduceCreateStockSubscriptionSuccessAction
        (
            StockSubscriptionsState state,
            CreateStockSubscriptionSuccessAction action
        )
            => new StockSubscriptionsState
                (
                    false,
                    null,
                    state.StockSubscriptionList,
                    state.StockSubscriptionReadModel,
                    state.PageNumber,
                    state.PageSize,
                    state.CreatePagePath,
                    state.SubscriptionListFilter,
                    action.StockSubscriptionCreateModel,
                    state.StockSubscriptionEditModel
                );

        [ReducerMethod]
        public static StockSubscriptionsState ReduceCreateStockSubscriptionFailureAction
        (
            StockSubscriptionsState state,
            CreateStockSubscriptionFailureAction action
        )
            => new StockSubscriptionsState
                (
                    false,
                    action.ErrorMessage,
                    state.StockSubscriptionList,
                    state.StockSubscriptionReadModel,
                    state.PageNumber,
                    state.PageSize,
                    state.CreatePagePath,
                    state.SubscriptionListFilter,
                    state.StockSubscriptionCreateModel,
                    state.StockSubscriptionEditModel
                );
    }
}
