

namespace PipefittersAccounting.SharedModel.Readmodels.Shared
{
    public class ExternalAgentReadModel
    {
        public Guid AgentId { get; set; }
        public int AgentTypeId { get; set; }
        public string? AgentTypeName { get; set; }
    }

    public class EconomicEventReadModel
    {
        public Guid EventId { get; set; }
        public int EventTypeId { get; set; }
        public string? EventTypeName { get; set; }
    }
}