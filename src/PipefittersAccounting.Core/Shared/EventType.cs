#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Shared
{
    public class EventType : Entity<int>
    {
        protected EventType() { }

        public EventType(int id, string eventName)
        {

        }

        public string EventTypeName { get; private set; }
    }
}