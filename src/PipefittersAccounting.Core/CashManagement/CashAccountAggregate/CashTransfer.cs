#pragma warning disable CS8618

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.CashManagement.CashAccountAggregate
{
    public class CashTransfer : Entity<Guid>
    {
        protected CashTransfer() { }

        public CashTransfer
        (
            EntityGuidID cashTransferId,
            EntityGuidID sourceAcctId,
            EntityGuidID destinationAcctId,
            CashTransactionAmount transferAmount,
            CashTransactionDate transferDate,
            EntityGuidID userId
        ) : this()
        {
            Id = cashTransferId;
            SourceCashAccountId = sourceAcctId;
            DestintaionCashAccountId = destinationAcctId;
            TransferAmount = transferAmount;
            TransactionDate = transferDate;
            UserId = userId;
        }

        public Guid SourceCashAccountId { get; init; }

        public Guid DestintaionCashAccountId { get; init; }

        public CashTransactionAmount TransferAmount { get; init; }

        public CashTransactionDate TransactionDate { get; init; }

        public Guid UserId { get; init; }

        protected override void CheckValidity()
        {
            if (SourceCashAccountId == DestintaionCashAccountId)
            {
                throw new InvalidOperationException("The source account and destination account must be different.");
            }
        }
    }
}