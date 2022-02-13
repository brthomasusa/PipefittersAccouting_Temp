using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Shared
{
    public class EconomicEvent : Entity<Guid>
    {
        protected EconomicEvent() { }

        private EconomicEvent(EntityGuidID id, EventTypeEnum eventType)
            : this()
        {
            Id = id;
            EventType = eventType;
        }

        public EventTypeEnum EventType { get; private set; }
    }
}