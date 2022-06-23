#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.HumanResources
{
    [Trait("Integration", "EfCoreCmdSvc")]
    public class EmployeeAggregateApplicationServiceTests : TestBaseEfCore
    {
        private readonly IEmployeeAggregateApplicationService _appService;

        public EmployeeAggregateApplicationServiceTests()
        {
            IEmployeeAggregateQueryService employeeQrySvc = new EmployeeAggregateQueryService(_dapperCtx);
            ISharedQueryService sharedQueryService = new SharedQueryService(_dapperCtx);
            IQueryServicesRegistry servicesRegistry = new QueryServicesRegistry();
            IEmployeeAggregateValidationService validationService =
                new EmployeeAggregateValidationService(employeeQrySvc, sharedQueryService, servicesRegistry);

            AppUnitOfWork unitOfWork = new AppUnitOfWork(_dbContext);
            IEmployeeAggregateRepository repository = new EmployeeAggregateRepository(_dbContext);
            _appService = new EmployeeAggregateApplicationService(validationService, repository, unitOfWork);
        }

        [Fact]
        public async Task CreateEmployeeInfo_EmployeeAggregateApplicationService_ShouldSucceed()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelCreate();

            OperationResult<bool> result = await _appService.CreateEmployeeInfo(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateEmployeeInfo_EmployeeAggregateApplicationService_EmployeeAlreadyExists_ShouldFail()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();

            OperationResult<bool> result = await _appService.CreateEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateEmployeeInfo_EmployeeAggregateApplicationService_DuplicateName_ShouldReturnFalse()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelCreate();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            OperationResult<bool> result = await _appService.CreateEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task EditEmployeeInfo_EmployeeAggregateApplicationService_ShouldSucceed()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();

            OperationResult<bool> result = await _appService.EditEmployeeInfo(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task EditEmployeeInfo_EmployeeAggregateApplicationService_InvalidEmployeeID_ShouldReturnFalse()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();
            model.EmployeeId = System.Guid.NewGuid();

            OperationResult<bool> result = await _appService.EditEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task EditEmployeeInfo_EmployeeAggregateApplicationService_DuplicateName_ShouldReturnFalse()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            OperationResult<bool> result = await _appService.EditEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteEmployeeInfo_EmployeeAggregateApplicationService_HasTimeCards_ShouldReturnFalse()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();

            OperationResult<bool> result = await _appService.DeleteEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateTimeCardInfo_EmployeeAggregateApplicationService_ShouldSucceed()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForCreate();

            OperationResult<bool> result = await _appService.CreateTimeCardInfo(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateTimeCardInfo_EmployeeAggregateApplicationService_InvalidEmpoyeeID_ShouldReturnFalse()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForCreate();
            model.EmployeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53");

            OperationResult<bool> result = await _appService.CreateTimeCardInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateTimeCardInfo_EmployeeAggregateApplicationService_InvalidSupvID_ShouldReturnFalse()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForCreate();
            model.SupervisorId = new Guid("5c60f693-bef5-e011-a485-80ee7300c695");

            OperationResult<bool> result = await _appService.CreateTimeCardInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateTimeCardInfo_EmployeeAggregateApplicationService_DuplicateDate_ShouldReturnFalse()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForCreate();
            model.PayPeriodEnded = new System.DateTime(2022, 2, 28);    // Invalid; date from previous pay period

            OperationResult<bool> result = await _appService.CreateTimeCardInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateTimeCardInfo_EmployeeAggregateApplicationService_NotEndOfMonth_ShouldReturnFalse()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForCreate();
            model.PayPeriodEnded = new System.DateTime(2022, 3, 28);    // Invalid; should be 2022-03-31

            OperationResult<bool> result = await _appService.CreateTimeCardInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task EditTimeCardInfo_EmployeeAggregateApplicationService_ShouldSucceed()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForEdit();

            OperationResult<bool> result = await _appService.EditTimeCardInfo(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task EditTimeCardInfo_EmployeeAggregateApplicationService_InvalidEmployeeId_ShouldReturnFalse()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForEdit();
            model.EmployeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53");

            OperationResult<bool> result = await _appService.EditTimeCardInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteTimeCardInfo_EmployeeAggregateApplicationService_ShouldSucceed()
        {
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForEdit();

            OperationResult<bool> result = await _appService.DeleteTimeCardInfo(model);

            Assert.True(result.Success);
        }












    }
}