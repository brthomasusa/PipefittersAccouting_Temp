#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Shared
{
    public class AgentType : Entity<int>
    {
        protected AgentType() { }

        public AgentType(int id, string name)
            : this()
        {
            Id = id;
            AgentTypeName = name;
        }

        public string AgentTypeName { get; private set; }
    }
}