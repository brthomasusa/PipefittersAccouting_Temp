#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Financing.FinancierAggregate;

namespace PipefittersAccounting.Core.Shared
{
    public class ExternalAgent : Entity<Guid>
    {
        protected ExternalAgent() { }

        public ExternalAgent(EntityGuidID id, AgentTypeEnum agentType)
            : this()
        {
            Id = id;
            AgentType = agentType;
        }

        public AgentTypeEnum AgentType { get; private set; }

        public virtual Employee Employee { get; private set; }
        public virtual Financier Financier { get; private set; }
    }
}