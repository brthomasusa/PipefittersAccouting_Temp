using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.Events
{
    public class CashDepositCreated : IDomainEvent
    {
        private CashDepositCreated(CashDeposit deposit) => CashDeposit = deposit;
        public CashDeposit CashDeposit { get; init; }

        public static CashDepositCreated Create(CashDeposit deposit)
            => new CashDepositCreated(deposit);
    }
}