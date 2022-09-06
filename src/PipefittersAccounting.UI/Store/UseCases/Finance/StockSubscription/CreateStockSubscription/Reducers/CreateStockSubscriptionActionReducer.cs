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
            => new StockSubscriptionsState(true, null, state.StockSubscriptionList, state.StockSubscriptionReadModel, state.PageNumber, state.PageSize);

        [ReducerMethod]
        public static StockSubscriptionsState ReduceCreateStockSubscriptionSuccessAction
        (
            StockSubscriptionsState state,
            CreateStockSubscriptionSuccessAction action
        )
            => new StockSubscriptionsState(false, null, action.StockSubscriptionList, action.StockSubscriptionReadModel, state.PageNumber, state.PageSize);

        [ReducerMethod]
        public static StockSubscriptionsState ReduceCreateStockSubscriptionFailureAction
        (
            StockSubscriptionsState state,
            CreateStockSubscriptionFailureAction action
        )
        {
            return new StockSubscriptionsState(false, action.ErrorMessage, state.StockSubscriptionList, state.StockSubscriptionReadModel, state.PageNumber, state.PageSize);
        }
    }
}
