#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class LoanPaymentEconEvent : ValueObject
    {
        protected LoanPaymentEconEvent() { }

        private LoanPaymentEconEvent(EconomicEvent evt)
            : this()
        {
            EconomicEvent = evt;
        }

        public EconomicEvent EconomicEvent { get; }

        public static LoanPaymentEconEvent Create(EntityGuidID id)
        {
            var evt = new EconomicEvent(id, EventTypeEnum.LoanPayment);
            return new LoanPaymentEconEvent(evt);
        }
    }
}