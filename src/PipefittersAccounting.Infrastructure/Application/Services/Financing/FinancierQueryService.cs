using PipefittersAccounting.Infrastructure.Application.Queries.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class FinancierQueryService : IFinancierQueryService
    {
        private readonly DapperContext _dapperCtx;

        public FinancierQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<FinancierDetail>> GetFinancierDetails(GetFinancier queryParameters)
            => await GetFinancierDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<FinancierListItems>>> GetFinancierListItems(GetFinanciers queryParameters)
            => await GetFinancierListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters)
            => await GetFinanciersLookupQuery.Query(queryParameters, _dapperCtx);
    }
}