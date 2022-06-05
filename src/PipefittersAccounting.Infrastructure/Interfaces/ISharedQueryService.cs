using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Shared;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface ISharedQueryService
    {

        Task<OperationResult<AgentIdentificationInfo>> GetExternalAgentIdentificationInfo(AgentIdentificationParameter queryParameters);

        Task<OperationResult<EventIdentificationInfo>> GetEconomicEventIdentificationInfo(EventIdentificationParameter queryParameters);
    }
}