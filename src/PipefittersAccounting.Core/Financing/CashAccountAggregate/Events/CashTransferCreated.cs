using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.Events
{
    public class CashTransferCreated : IDomainEvent
    {
        private CashTransferCreated(CashTransfer cashTransfer) => CashTransfer = cashTransfer;

        public CashTransfer CashTransfer { get; init; }

        public static CashTransferCreated Create(CashTransfer cashTransfer)
            => new CashTransferCreated(cashTransfer);
    }
}