#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects
{
    public class LoanAgreementEconEvent : ValueObject
    {
        protected LoanAgreementEconEvent() { }

        private LoanAgreementEconEvent(EconomicEvent evt)
            : this()
        {
            EconomicEvent = evt;
        }

        public EconomicEvent EconomicEvent { get; }

        public static LoanAgreementEconEvent Create(EntityGuidID id)
        {
            var evt = new EconomicEvent(id, EventTypeEnum.LoanAgreementCashReceipt);
            return new LoanAgreementEconEvent(evt);
        }
    }
}