using PipefittersAccounting.Infrastructure.Application.Queries.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Shared;

namespace PipefittersAccounting.Infrastructure.Application.Services.Shared
{
    public class SharedQueryService : ISharedQueryService
    {
        private readonly DapperContext _dapperCtx;

        public SharedQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<AgentIdentificationInfo>> GetExternalAgentIdentificationInfo(AgentIdentificationParameter queryParameters)
            => await GetAgentIdentificationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<EventIdentificationInfo>> GetEconomicEventIdentificationInfo(EventIdentificationParameter queryParameters)
            => await GetEventIdentificationInfoQuery.Query(queryParameters, _dapperCtx);
    }
}
