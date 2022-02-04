#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Core.Shared
{
    public class ExternalAgent : Entity<Guid>
    {
        protected ExternalAgent() { }

        public ExternalAgent(EntityGuidID id, ExternalAgentType agentType)
            : this()
        {
            AgentId = id;
            AgentType = agentType.AgentType;
        }

        public Guid AgentId { get; private set; }

        public AgentType AgentType { get; private set; }

    }
}