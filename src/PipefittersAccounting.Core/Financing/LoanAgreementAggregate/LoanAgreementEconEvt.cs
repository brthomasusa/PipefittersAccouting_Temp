#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Financing.LoanAgreementAggregate
{
    public class LoanAgreementEconEvt : ValueObject
    {
        protected LoanAgreementEconEvt() { }

        private LoanAgreementEconEvt(EconomicEvent evt)
            : this()
        {
            EconomicEvent = evt;
        }

        public EconomicEvent EconomicEvent { get; }

        public static LoanAgreementEconEvt Create(EntityGuidID id)
        {
            var evt = new EconomicEvent(id, EventTypeEnum.LoanAgreementCashReceipt);
            return new LoanAgreementEconEvt(evt);
        }
    }
}