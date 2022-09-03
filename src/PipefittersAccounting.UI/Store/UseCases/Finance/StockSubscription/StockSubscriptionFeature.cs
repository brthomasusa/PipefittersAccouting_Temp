using Fluxor;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription
{
    public class StockSubscriptionFeature : Feature<StockSubscriptionState>
    {
        public override string GetName() => "StockSubscriptionFeature";

        protected override StockSubscriptionState GetInitialState() =>
            new StockSubscriptionState(false, null, null, null);
    }
}