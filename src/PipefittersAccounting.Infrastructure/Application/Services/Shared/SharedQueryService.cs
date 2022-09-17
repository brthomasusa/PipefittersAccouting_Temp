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

        public async Task<OperationResult<ExternalAgentReadModel>> GetExternalAgentIdentificationInfo(ExternalAgentParameter queryParameters)
            => await GetAgentIdentificationInfoQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<EconomicEventReadModel>> GetEconomicEventIdentificationInfo(EconomicEventParameter queryParameters)
            => await GetEventIdentificationInfoQuery.Query(queryParameters, _dapperCtx);
    }
}
