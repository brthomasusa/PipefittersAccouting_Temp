using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.State.Finance.StockSubscription
{
    public class StockSubscriptionsState : RootState
    {
        public StockSubscriptionsState
        (
            bool isLoading,
            string? currentErrorMessage,
            PagingResponse<StockSubscriptionListItem>? currentSubscriptions,
            StockSubscriptionReadModel? currentSubscription,
            int pageNumber,
            int pageSize,
            string createPageHref
        ) : base(isLoading, currentErrorMessage)
        {
            StockSubscriptionList = currentSubscriptions;
            StockSubscriptionReadModel = currentSubscription;
            PageNumber = pageNumber;
            PageSize = pageSize;
            CreatePagePath = createPageHref;
        }

        public PagingResponse<StockSubscriptionListItem>? StockSubscriptionList { get; init; }

        public StockSubscriptionReadModel? StockSubscriptionReadModel { get; init; }
        public StockSubscriptionWriteModel? StockSubscriptionCreateModel { get; init; }
        public StockSubscriptionWriteModel? StockSubscriptionEditModel { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string CreatePagePath { get; init; }
    }
}