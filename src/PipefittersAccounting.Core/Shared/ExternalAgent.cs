#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Core.Shared
{
    public class ExternalAgent : Entity<Guid>
    {
        protected ExternalAgent() { }

        public ExternalAgent(EntityGuidID id, AgentTypeEnum agentType)
            : this()
        {
            AgentId = id;
            AgentType = agentType;
        }

        public Guid AgentId { get; private set; }

        public AgentTypeEnum AgentType { get; private set; }

        public virtual Employee Employee { get; private set; }
    }
}