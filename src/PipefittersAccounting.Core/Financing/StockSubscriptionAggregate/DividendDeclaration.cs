#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.StockSubscriptionAggregate
{
    public class DividendDeclaration : Entity<Guid>
    {
        protected DividendDeclaration() { }

        public DividendDeclaration
        (
            EntityGuidID dividendId,
            EntityGuidID stockId,
            DividendDeclarationDate dividendDeclarationDate,
            DividendPerShare dividendPerShare,
            EntityGuidID userId
        )
            : this()
        {
            Id = dividendId;
            StockId = stockId;
            DividendDeclarationDate = dividendDeclarationDate;
            DividendPerShare = dividendPerShare;
            UserId = userId;
        }

        public virtual EntityGuidID StockId { get; private set; }

        public virtual DividendDeclarationDate DividendDeclarationDate { get; private set; }
        public void UpdateDividendDeclarationDate(DividendDeclarationDate value)
        {
            DividendDeclarationDate = value ?? throw new ArgumentNullException("The dividend declaration date is required.");
        }

        public virtual DividendPerShare DividendPerShare { get; set; }
        public void UpdateDividendPerShare(DividendPerShare value)
        {
            DividendPerShare = value ?? throw new ArgumentNullException("The dividend per share is required.");
        }

        public EntityGuidID UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }
    }
}