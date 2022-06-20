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
            GetEmployeesParameters queryParameters = new GetEmployeesParameters() { Page = 1, PageSize = 15 };
            OperationResult<PagedList<EmployeeListItem>> result = await _queryService.GetEmployeeListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(13, result.Result.Count);
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

        [Fact]
        public async Task GetCountOfEmployeeTimeCards_EmployeeAggregateQueryService_ShouldReturn2()
        {
            GetEmployeeParameter qryParam =
                new() { EmployeeID = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53") };

            OperationResult<int> result = await _queryService.GetCountOfEmployeeTimeCards(qryParam);

            Assert.True(result.Success);
            Assert.Equal(2, result.Result);
        }

        [Fact]
        public async Task GetEmployeeTimeCardDetails_EmployeeAggregateQueryService_ShouldReturnTrue()
        {
            GetTimeCardParameter qryParam =
                new() { TimeCardId = new Guid("d4ad0ad8-7e03-4bb2-8ce0-04e5e95428a1") };

            OperationResult<TimeCardDetail> result = await _queryService.GetEmployeeTimeCardDetails(qryParam);

            Assert.True(result.Success);
            Assert.Equal("Roberto W Tamburello", result.Result.EmployeeFullName);
        }

        [Fact]
        public async Task GetEmployeeTimeCardListItems_EmployeeAggregateQueryService_ShouldReturnTrue()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52") };
            OperationResult<List<TimeCardListItem>> result = await _queryService.GetEmployeeTimeCardListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(2, result.Result.Count);
        }

        [Fact]
        public async Task VerifyEmployeeSupervisorLink_EmployeeAggregateQueryService_ShouldReturnTrue()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52") };
            OperationResult<Guid> result = await _queryService.VerifyEmployeeSupervisorLink(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new Guid("aedc617c-d035-4213-b55a-dae5cdfca366"), result.Result);
        }

        [Fact]
        public async Task VerifyEmployeeSupervisorLink_EmployeeAggregateQueryService_InvalidEmployeeID_ShouldReturnFalse()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f") };
            OperationResult<Guid> result = await _queryService.VerifyEmployeeSupervisorLink(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task GetMostRecentPayPeriodEndedDate_EmployeeAggregateQueryService_ShouldReturn20220228()
        {
            GetEmployeeParameter queryParameters = new() { EmployeeID = new Guid("c40888a1-c182-437e-9c1d-e9227bca7f52") };
            OperationResult<DateTime> result = await _queryService.GetMostRecentPayPeriodEndedDate(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(2022, 2, 28), result.Result);
        }

        [Fact]
        public async Task GetTimeCardPaymentVerification_EmployeeAggregateQueryService_ShouldReturnTrue()
        {
            GetTimeCardParameter queryParameters = new() { TimeCardId = new Guid("d4ad0ad8-7e03-4bb2-8ce0-04e5e95428a1") };
            OperationResult<TimeCardPaymentVerification> result = await _queryService.GetTimeCardPaymentVerification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(168, result.Result.RegularHours);
            Assert.Equal(new DateTime(), result.Result.DatePaid);
        }

        [Fact]
        public async Task GetPayrollRegister_EmployeeAggregateQueryService_ShouldReturnTrue()
        {
            GetPayrollRegisterParameter queryParameters = new() { PayPeriodEnded = new DateTime(2022, 2, 28) };
            OperationResult<List<PayrollRegister>> result = await _queryService.GetPayrollRegister(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(13, result.Result.Count);
        }
    }
}