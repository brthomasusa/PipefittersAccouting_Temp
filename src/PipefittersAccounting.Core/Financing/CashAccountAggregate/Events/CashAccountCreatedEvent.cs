using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.Events
{
    public class CashAccountCreatedEvent : IDomainEvent
    {
        public CashAccountCreatedEvent(CashAccount account) => CashAccount = account;

        public CashAccount CashAccount { get; init; }
    }
}
