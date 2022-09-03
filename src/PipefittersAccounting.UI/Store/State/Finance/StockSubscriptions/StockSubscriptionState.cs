using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.State.Finance.StockSubscription
{
    public class StockSubscriptionState : RootState
    {
        public StockSubscriptionState
        (
            bool isLoading,
            string? currentErrorMessage,
            PagingResponse<StockSubscriptionListItem>? currentSubscriptions,
            StockSubscriptionReadModel? currentSubscription
        ) : base(isLoading, currentErrorMessage)
        {
            CurrentSubscriptions = currentSubscriptions;
            CurrentSubscription = currentSubscription;
        }

        public PagingResponse<StockSubscriptionListItem>? CurrentSubscriptions { get; }

        public StockSubscriptionReadModel? CurrentSubscription { get; }
    }
}