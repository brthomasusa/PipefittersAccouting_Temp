using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.HumanResources
{
    public class EmployeeQueryServiceDapperTests : TestBaseDapper
    {
        [Fact]
        public async Task Dapper_GetEmployeeDetails_ShouldSucceed()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            Guid agentID = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployee qryParam = new GetEmployee() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> result = await queryService.GetEmployeeDetails(qryParam);

            Assert.True(result.Success);
            Assert.Equal(agentID, result.Result.EmployeeId);
            Assert.Equal("Ken J Sanchez", result.Result.ManagerFullName);
            Assert.Equal("Ken J Sanchez", result.Result.EmployeeFullName);
        }

        [Fact]
        public async Task Dapper_GetEmployeeDetails_WithBadEmployeeId_ShouldFail()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            Guid agentID = new Guid("12300A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployee qryParam = new GetEmployee() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> result = await queryService.GetEmployeeDetails(qryParam);

            Assert.False(result.Success);
            Assert.IsType<ArgumentException>(result.Exception);
            string errMsg = $"No employee record found where EmployeeId equals '{qryParam.EmployeeID}'.";
            Assert.Equal(errMsg, result.Exception.Message);
        }

        [Fact]
        public async Task Dapper_GetEmployeeManagers_ShouldSucceed()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            GetEmployeeManagers queryParameters = new GetEmployeeManagers();
            OperationResult<List<EmployeeManager>> result = await queryService.GetEmployeeManagers(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(3, result.Result.Count);
        }

        [Fact]
        public async Task Dapper_ShouldReturn_PagedList_EmployeeListItems_ReadModel()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            GetEmployees queryParameters = new GetEmployees() { Page = 1, PageSize = 10 };
            OperationResult<PagedList<EmployeeListItem>> result = await queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(9, result.Result.Count);
        }

        [Fact]
        public async Task Dapper_GetEmployees_WithPagination_ShouldSucceed()
        {
            IEmployeeAggregateQueryService queryService = new EmployeeAggregateQueryServiceDapper(_dapperCtx);

            // Get page 1 of 2
            GetEmployees queryParameters = new GetEmployees() { Page = 1, PageSize = 5 };
            OperationResult<PagedList<EmployeeListItem>> result = await queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(5, result.Result.Count);

            // Get page 2 of 2
            queryParameters = new GetEmployees() { Page = 2, PageSize = 5 };
            result = await queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(4, result.Result.Count);
        }
    }
}