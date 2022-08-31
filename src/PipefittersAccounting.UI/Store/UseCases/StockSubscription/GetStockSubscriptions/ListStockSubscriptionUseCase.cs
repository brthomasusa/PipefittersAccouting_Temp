using Fluxor;
using PipefittersAccounting.UI.Store.State;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions
{
    public class ListStockSubscriptionUseCase : Feature<StockSubscriptionState>
    {
        public override string GetName() => "ListStockSubscriptions";

        protected override StockSubscriptionState GetInitialState() =>
            new StockSubscriptionState(false, null, null, null);
    }
}