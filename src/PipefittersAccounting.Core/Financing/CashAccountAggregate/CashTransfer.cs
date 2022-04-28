#pragma warning disable CS8618

using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashTransfer : Entity<Guid>
    {
        protected CashTransfer() { }

        public CashTransfer
        (
            EntityGuidID sourceAcctId,
            EntityGuidID destinationAcctId,
            CashTransactionAmount transferAmount,
            CashTransactionDate transferDate,
            EntityGuidID userId
        ) : this()
        {
            SourceCashAccountId = sourceAcctId;
            DestintaionCashAccountId = destinationAcctId;
            TransferAmount = transferAmount;
            TransactionDate = transferDate;
            UserId = userId;
        }

        public EntityGuidID SourceCashAccountId { get; init; }

        public EntityGuidID DestintaionCashAccountId { get; init; }

        public CashTransactionAmount TransferAmount { get; init; }

        public CashTransactionDate TransactionDate { get; init; }

        public EntityGuidID UserId { get; init; }

        protected override void CheckValidity()
        {
            if (SourceCashAccountId == DestintaionCashAccountId)
            {
                throw new InvalidOperationException("The source account and destination account must be different.");
            }
        }
    }
}