
using PipefittersAccounting.Infrastructure.Application.Queries.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateQueryService : IEmployeeAggregateQueryService
    {
        private readonly DapperContext _dapperCtx;

        public EmployeeAggregateQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters) =>
            await GetEmployeeDetailQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters) =>
            await GetEmployeeListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagersParameters queryParameters) =>
            await GetEmployeeManagersQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyEmployeeSSNIsUnique(UniqueEmployeSSNParameter queryParameters)
            => await VerifyEmployeeSSNIsUniqueQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyEmployeeNameIsUnique(UniqueEmployeeNameParameters queryParameters)
            => await VerifyEmployeeNameIsUniqueQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<int>> GetCountOfEmployeeTimeCards(GetEmployeeParameter queryParameters)
            => await GetCountOfEmployeeTimeCardsQuery.Query(queryParameters, _dapperCtx);
    }
}