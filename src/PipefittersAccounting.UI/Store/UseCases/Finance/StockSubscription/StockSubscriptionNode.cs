using Fluxor;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription
{
    public class StockSubscriptionNode : Feature<StockSubscriptionsState>
    {
        public override string GetName() => "StockSubscriptionNode";

        protected override StockSubscriptionsState GetInitialState() =>
            new StockSubscriptionsState
                (
                    false,
                    null,
                    null,
                    null,
                    1,
                    5,
                    @"/Finance/Pages/StockSubscriptions/StockSubscriptionCreatePage",
                    "all"
                );
    }
}