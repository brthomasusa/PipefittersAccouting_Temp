using Fluxor;

namespace PipefittersAccounting.UI.Store.StockSubscription.CreateStockSubscriptionUseCase
{
    [FeatureState]
    public class StockSubscriptionState
    {
        private StockSubscriptionState() { }

        public StockSubscriptionState
        (
            Guid stockId,
            Guid financierId,
            DateTime stockIssueDate,
            int sharesIssued,
            decimal pricePerShare,
            Guid userId
        )
        {
            StockId = stockId;
            FinancierId = financierId;
            StockIssueDate = stockIssueDate;
            SharesIssued = sharesIssued;
            PricePerShare = pricePerShare;
            UserId = userId;
        }

        public Guid StockId { get; }
        public Guid FinancierId { get; }
        public DateTime StockIssueDate { get; }
        public int SharesIssued { get; }
        public decimal PricePerShare { get; }
        public Guid UserId { get; }
    }
}