using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Shared
{
    public class ExternalAgentType : ValueObject
    {
        protected ExternalAgentType() { }

        private ExternalAgentType(AgentType agentType)
            : this()
        {
            this.AgentType = agentType;
        }

        public AgentType AgentType { get; }

        public static ExternalAgentType Create(AgentType agentType)
        {
            CheckValidity(agentType);
            return new ExternalAgentType(agentType);
        }

        private static void CheckValidity(AgentType agentType)
        {
            if (!Enum.IsDefined(typeof(AgentType), agentType))
            {
                throw new ArgumentOutOfRangeException("Undefined external agent type.", nameof(agentType));
            }
        }
    }
}