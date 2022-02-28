
using PipefittersAccounting.Infrastructure.Application.Queries.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateQueryServiceDapper : IEmployeeAggregateQueryService
    {
        private readonly DapperContext _dapperCtx;

        public EmployeeAggregateQueryServiceDapper(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployee queryParameters) =>
            await GetEmployeeDetailQueryDapper.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployees queryParameters) =>
            await GetEmployeeListItemsQueryDapper.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagers queryParameters) =>
            await GetEmployeeManagersQueryDapper.Query(queryParameters, _dapperCtx);
    }
}