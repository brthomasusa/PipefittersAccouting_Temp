#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Core.HumanResources.EmployeeAggregate
{
    public class EmployeeAgent : ValueObject
    {
        protected EmployeeAgent() { }

        private EmployeeAgent(ExternalAgent agent)
        {
            ExternalAgent = agent;
        }

        public ExternalAgent ExternalAgent { get; }

        public static EmployeeAgent Create(EntityGuidID id)
        {
            var agent = new ExternalAgent(id, AgentTypeEnum.Employee);
            return new EmployeeAgent(agent);
        }
    }
}