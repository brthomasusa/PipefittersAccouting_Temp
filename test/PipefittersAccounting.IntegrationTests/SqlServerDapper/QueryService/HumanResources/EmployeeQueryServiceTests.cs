using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.HumanResources
{
    [Trait("Integration", "DapperQueryService")]
    public class EmployeeQueryServiceTests : TestBaseDapper
    {
        IEmployeeAggregateQueryService _queryService;

        public EmployeeQueryServiceTests()
            => _queryService = new EmployeeAggregateQueryService(_dapperCtx);

        [Fact]
        public async Task GetEmployeeDetails_EmployeeAggregateQueryService_ShouldSucceed()
        {
            Guid agentID = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployeeParameter qryParam = new GetEmployeeParameter() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> result = await _queryService.GetEmployeeDetails(qryParam);

            Assert.True(result.Success);
            Assert.Equal(agentID, result.Result.EmployeeId);
            Assert.Equal("Ken J Sanchez", result.Result.ManagerFullName);
            Assert.Equal("Ken J Sanchez", result.Result.EmployeeFullName);
        }

        [Fact]
        public async Task GetEmployeeDetails_EmployeeAggregateQueryService_WithBadEmployeeId_ShouldFail()
        {
            Guid agentID = new Guid("12300A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployeeParameter qryParam = new GetEmployeeParameter() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> result = await _queryService.GetEmployeeDetails(qryParam);

            Assert.False(result.Success);
            string errMsg = $"No employee record found where EmployeeId equals '{qryParam.EmployeeID}'.";
            Assert.Equal(errMsg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetEmployeeManagers_EmployeeAggregateQueryService_ShouldSucceed()
        {
            GetEmployeeManagersParameters queryParameters = new GetEmployeeManagersParameters();
            OperationResult<List<EmployeeManager>> result = await _queryService.GetEmployeeManagers(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(3, result.Result.Count);
        }

        [Fact]
        public async Task GetEmployeeListItem_EmployeeAggregateQueryService_EmployeeListItems_ReadModel()
        {
            GetEmployeesParameters queryParameters = new GetEmployeesParameters() { Page = 1, PageSize = 10 };
            OperationResult<PagedList<EmployeeListItem>> result = await _queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(9, result.Result.Count);
        }

        [Fact]
        public async Task GetEmployees_EmployeeAggregateQueryService_WithPagination_ShouldSucceed()
        {
            // Get page 1 of 2
            GetEmployeesParameters queryParameters = new GetEmployeesParameters() { Page = 1, PageSize = 5 };
            OperationResult<PagedList<EmployeeListItem>> result = await _queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(5, result.Result.Count);

            // Get page 2 of 2
            queryParameters = new GetEmployeesParameters() { Page = 2, PageSize = 5 };
            result = await _queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(4, result.Result.Count);
        }

        [Fact]
        public async Task VerifyEmployeeSSNIsUnique_EmployeeAggregateQueryService_ShouldReturnEmptyGuid()
        {
            UniqueEmployeSSNParameter qryParam = new() { SSN = "123781239" };

            OperationResult<Guid> result = await _queryService.VerifyEmployeeSSNIsUnique(qryParam);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task VerifyEmployeeSSNIsUnique_EmployeeAggregateQueryService_ShouldReturnEmployeeId()
        {
            UniqueEmployeSSNParameter qryParam = new() { SSN = "825559874" };

            OperationResult<Guid> result = await _queryService.VerifyEmployeeSSNIsUnique(qryParam);

            Assert.True(result.Success);
            Assert.Equal(new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5"), result.Result);
        }

        [Fact]
        public async Task VerifyEmployeeNameIsUnique_EmployeeAggregateQueryService_ShouldReturnEmptyGuid()
        {
            UniqueEmployeeNameParameters qryParam =
                new() { LastName = "Scott", FirstName = "Travis", MiddleInitial = "Z" };

            OperationResult<Guid> result = await _queryService.VerifyEmployeeNameIsUnique(qryParam);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task VerifyEmployeeNameIsUnique_EmployeeAggregateQueryService_ShouldReturnEmployeeId()
        {
            UniqueEmployeeNameParameters qryParam =
                new() { LastName = "Duffy", FirstName = "Terri", MiddleInitial = "L" };

            OperationResult<Guid> result = await _queryService.VerifyEmployeeNameIsUnique(qryParam);

            Assert.True(result.Success);
            Assert.Equal(new Guid("9f7b902d-566c-4db6-b07b-716dd4e04340"), result.Result);
        }
    }
}