#pragma warning disable CS8604

using System.Collections.Generic;
using System.Threading.Tasks;

using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.Sqlite.QueryHandling.HumanResources
{
    public class EmployeeQueryServiceSqlite : IEmployeeAggregateQueryService
    {
        private readonly AppDbContext _dbContext;

        public EmployeeQueryServiceSqlite(AppDbContext ctx) => _dbContext = ctx;

        public async Task<OperationResult<EmployeeDetail>> Query(GetEmployee queryParameters) =>
            await GetEmployeeDetailQuery.Query(queryParameters, _dbContext);

        public Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployees queryParameters) =>
            GetEmployeeListItemsQuery.Query(queryParameters, _dbContext);

        public Task<OperationResult<List<EmployeeManager>>> Query(GetEmployeeManagers queryParameters) =>
            GetEmployeeManagersQuery.Query(queryParameters, _dbContext);
    }
}