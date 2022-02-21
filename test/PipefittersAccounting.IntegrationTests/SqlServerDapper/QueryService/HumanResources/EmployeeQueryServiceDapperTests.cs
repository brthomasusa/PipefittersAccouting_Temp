using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.HumanResources
{
    public class EmployeeQueryServiceDapperTests : TestBaseDapper
    {
        [Fact]
        public async Task Dapper_ShouldReturn_EmployeeDetail_ReadModel()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            Guid agentID = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployee qryParam = new GetEmployee() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> result = await queryService.Query(qryParam);

            Assert.True(result.Success);
            Assert.Equal(agentID, result.Result.EmployeeId);
            Assert.Equal("Ken J Sanchez", result.Result.ManagerFullName);
            Assert.Equal("Ken J Sanchez", result.Result.EmployeeFullName);
        }

        [Fact]
        public async Task Dapper_ShouldReturn_Failure_With_Bad_EmployeeId()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            Guid agentID = new Guid("12300A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployee qryParam = new GetEmployee() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> result = await queryService.Query(qryParam);

            Assert.False(result.Success);
            Assert.IsType<ArgumentException>(result.Exception);
            string errMsg = $"No employee record found where EmployeeId equals '{qryParam.EmployeeID}'.";
            Assert.Equal(errMsg, result.Exception.Message);
        }

        [Fact]
        public async Task Dapper_ShouldReturn_EmployeeManagers_ReadModel()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            GetEmployeeManagers queryParameters = new GetEmployeeManagers();
            OperationResult<List<EmployeeManager>> result = await queryService.Query(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(3, result.Result.Count);
        }

        [Fact]
        public async Task Dapper_ShouldReturn_PagedList_EmployeeListItems_ReadModel()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            GetEmployees queryParameters = new GetEmployees() { Page = 1, PageSize = 10 };
            OperationResult<PagedList<EmployeeListItem>> result = await queryService.Query(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(9, result.Result.ReadModels.Count);
        }

        [Fact]
        public async Task Dapper_ShouldReturn_PagedList_EmployeeListItems_ReadModel_Pagination()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            GetEmployees queryParameters = new GetEmployees() { Page = 1, PageSize = 5 };
            OperationResult<PagedList<EmployeeListItem>> result = await queryService.Query(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(5, result.Result.ReadModels.Count);

            queryParameters = new GetEmployees() { Page = 2, PageSize = 5 };
            result = await queryService.Query(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(4, result.Result.ReadModels.Count);
        }
    }
}