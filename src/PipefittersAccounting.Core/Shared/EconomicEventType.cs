using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Shared
{
    public class EconomicEventType : ValueObject
    {
        protected EconomicEventType() { }

        private EconomicEventType(EventType eventType)
            : this()
        {
            this.EventType = eventType;
        }

        public EventType EventType { get; }

        public static EconomicEventType Create(EventType eventType)
        {
            CheckValidity(eventType);
            return new EconomicEventType(eventType);
        }

        private static void CheckValidity(EventType eventType)
        {
            if (!Enum.IsDefined(typeof(EventType), eventType))
            {
                throw new ArgumentOutOfRangeException("Undefined economic event type.", nameof(eventType));
            }
        }
    }
}