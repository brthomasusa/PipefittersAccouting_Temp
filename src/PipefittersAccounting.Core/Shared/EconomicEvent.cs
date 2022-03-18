#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;

namespace PipefittersAccounting.Core.Shared
{
    public class EconomicEvent : Entity<Guid>
    {
        protected EconomicEvent() { }

        public EconomicEvent(EntityGuidID id, EventTypeEnum eventType)
            : this()
        {
            Id = id;
            EventType = eventType;
        }

        public EventTypeEnum EventType { get; private set; }

        public virtual LoanAgreement LoanAgreement { get; private set; }
        public virtual LoanInstallment LoanInstallment { get; private set; }

        // public virtual StockSubscription StockSubscription { get; private set; }        
    }
}