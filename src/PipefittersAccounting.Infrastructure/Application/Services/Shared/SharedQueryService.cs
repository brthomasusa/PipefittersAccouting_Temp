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

        public Task<OperationResult<AgentIdentificationInfo>> GetExternalAgentIdentificationInfo(AgentIdentificationParameter queryParameters)
            => throw new NotImplementedException();

        public Task<OperationResult<EventIdentificationInfo>> GetEconomicEventIdentificationInfo(EventIdentificationParameter queryParameters)
            => throw new NotImplementedException();
    }
}