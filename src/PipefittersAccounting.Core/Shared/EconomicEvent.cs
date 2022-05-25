#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

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

        public EventTypeEnum EventType { get; protected set; }
    }

    public enum EventTypeEnum : int
    {
        Sales = 1,
        LoanAgreement = 2,
        StockSubscription = 3,
        LoanPayment = 4,
        DividentPayment = 5,
        TimeCardPayment = 6,
        InventoryReceipt = 7,
        CashTransfer = 8
    }
}