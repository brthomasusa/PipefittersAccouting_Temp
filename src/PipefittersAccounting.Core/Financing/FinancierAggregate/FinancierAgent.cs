#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Financing.FinancierAggregate
{
    public class FinancierAgent : ValueObject
    {
        protected FinancierAgent() { }

        private FinancierAgent(ExternalAgent agent)
            : this()
        {
            ExternalAgent = agent;
        }

        public ExternalAgent ExternalAgent { get; }

        public static FinancierAgent Create(EntityGuidID id)
        {
            var agent = new ExternalAgent(id, AgentTypeEnum.Financier);
            return new FinancierAgent(agent);
        }
    }
}