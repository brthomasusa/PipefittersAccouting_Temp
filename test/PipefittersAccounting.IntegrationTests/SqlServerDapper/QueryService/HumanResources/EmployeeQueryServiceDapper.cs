using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using PipefittersAccounting.Infrastructure.Application.Queries.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.HumanResources
{
    public class EmployeeQueryServiceDapper : IEmployeeAggregateQueryService
    {
        private readonly DapperContext _dapperCtx;

        public EmployeeQueryServiceDapper(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<EmployeeDetail>> Query(GetEmployee queryParameters) =>
            await GetEmployeeDetailQueryDapper.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployees queryParameters) =>
            await GetEmployeeListItemsQueryDapper.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<List<EmployeeManager>>> Query(GetEmployeeManagers queryParameters) =>
            await GetEmployeeManagersQueryDapper.Query(queryParameters, _dapperCtx);
    }
}