using Fluxor;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;
using PipefittersAccounting.UI.Store.UseCases.StockSubscription.CreateStockSubscription.Actions;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.CreateStockSubscription.Reducers
{
    public static class CreateStockSubscriptionActionReducer
    {
        [ReducerMethod]
        public static StockSubscriptionState ReduceCreateStockSubscriptionAction
        (
            StockSubscriptionState state,
            CreateStockSubscriptionAction _
        )
            => new StockSubscriptionState(true, null, state.CurrentSubscriptions, state.CurrentSubscription);


    }
}