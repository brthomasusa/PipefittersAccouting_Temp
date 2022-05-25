#pragma warning disable CS8618

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.StockSubscriptionAggregate
{
    public class StockSubscription : AggregateRoot<Guid>, IAggregateRoot
    {
        protected StockSubscription() { }

        public StockSubscription
        (
            EntityGuidID stockId,
            EntityGuidID financierId,
            StockIssueDate stockIssueDate,
            SharesIssured sharesIssured,
            PricePerShare pricePerShare,
            EntityGuidID userId
        )
            : this()
        {
            Id = stockId ?? throw new ArgumentNullException("The stock subscription id is required.");
            FinancierId = financierId ?? throw new ArgumentNullException("The financier id is required.");
        }

        public EntityGuidID FinancierId { get; private set; }

        public virtual SharesIssured SharesIssured { get; private set; }
        public void UpdateSharesIssured(SharesIssured value)
        {
            SharesIssured = value ?? throw new ArgumentNullException("The number of shares issued is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual PricePerShare PricePerShare { get; private set; }
        public void UpdatePricePerShare(PricePerShare value)
        {
            PricePerShare = value ?? throw new ArgumentNullException("The price per share is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public virtual StockIssueDate StockIssueDate { get; private set; }
        public void UpdateStockIssueDate(StockIssueDate value)
        {
            StockIssueDate = value ?? throw new ArgumentNullException("The stock issue date is required.");
            UpdateLastModifiedDate();
            CheckValidity();
        }

    }
}