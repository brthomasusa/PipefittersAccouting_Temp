#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Financing.StockSubscriptionAggregate
{
    public class StockSubscription : AggregateRoot<Guid>
    {
        private List<DividendDeclaration> _dividendDeclarations = new();

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
            StockIssueDate = stockIssueDate ?? throw new ArgumentNullException("The stock issue date is required.");
            SharesIssured = sharesIssured ?? throw new ArgumentNullException("The number of shares issued is required.");
            PricePerShare = pricePerShare ?? throw new ArgumentNullException("The price per share is required.");
            UserId = userId ?? throw new ArgumentNullException("The user id is required.");
        }

        public virtual EntityGuidID FinancierId { get; init; }

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

        public EntityGuidID UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        public virtual IReadOnlyCollection<DividendDeclaration> DividendDeclarations => _dividendDeclarations.ToList();

        public OperationResult<bool> AddDividendDeclaration
        (
            Guid dividendId,
            DateTime dividendDeclarationDate,
            decimal dividendPerShare,
            Guid userId
        )
        {
            if (dividendId == Guid.Empty)
                return OperationResult<bool>.CreateFailure("A dividend id is required.");

            if (dividendDeclarationDate < StockIssueDate)
                return OperationResult<bool>.CreateFailure("Dividend declaration date must be greater than stock issue date.");

            try
            {
                _dividendDeclarations.Add(new DividendDeclaration
                (
                    EntityGuidID.Create(dividendId),
                    EntityGuidID.Create(this.Id),
                    DividendDeclarationDate.Create(dividendDeclarationDate),
                    DividendPerShare.Create(dividendPerShare),
                    EntityGuidID.Create(userId)
                ));

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        public OperationResult<bool> EditDividendDeclaration
        (
            Guid dividendId,
            DateTime dividendDeclarationDate,
            decimal dividendPerShare,
            Guid userId
        )
        {
            if (dividendDeclarationDate < StockIssueDate)
                return OperationResult<bool>.CreateFailure("Dividend declaration date must be greater than stock issue date.");

            DividendDeclaration? dividend
                = _dividendDeclarations.Find(x => x.Id == dividendId);

            if (dividend is null)
                return OperationResult<bool>.CreateFailure($"Update failed! Unable to locate a dividend declaration with id '{dividendId}'");

            try
            {
                dividend.UpdateDividendDeclarationDate(DividendDeclarationDate.Create(dividendDeclarationDate));
                dividend.UpdateDividendPerShare(DividendPerShare.Create(dividendPerShare));
                dividend.UpdateUserId(EntityGuidID.Create(userId));

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}