using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.Events
{
    public class CashDisbursementCreated : IDomainEvent
    {
        private CashDisbursementCreated(CashDisbursement disbursement) => CashDisbursement = disbursement;
        public CashDisbursement CashDisbursement { get; init; }

        public static CashDisbursementCreated Create(CashDisbursement disbursement)
            => new CashDisbursementCreated(disbursement);
    }
}