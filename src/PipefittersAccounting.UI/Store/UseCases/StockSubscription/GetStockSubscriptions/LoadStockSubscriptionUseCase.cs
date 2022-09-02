using Fluxor;
using PipefittersAccounting.UI.Store.State.StockSubscription;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions
{
    public class LoadStockSubscriptionUseCase : Feature<StockSubscriptionState>
    {
        public override string GetName() => "LoadStockSubscriptions";

        protected override StockSubscriptionState GetInitialState() =>
            new StockSubscriptionState(false, null, null, null);
    }
}