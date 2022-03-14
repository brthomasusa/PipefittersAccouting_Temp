#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanPaymentEconEvt : ValueObject
    {
        protected LoanPaymentEconEvt() { }

        private LoanPaymentEconEvt(EconomicEvent evt)
            : this()
        {
            EconomicEvent = evt;
        }

        public EconomicEvent EconomicEvent { get; }

        public static LoanPaymentEconEvt Create(EntityGuidID id)
        {
            var evt = new EconomicEvent(id, EventTypeEnum.CashDisbursementForLoanPayment);
            return new LoanPaymentEconEvt(evt);
        }
    }
}