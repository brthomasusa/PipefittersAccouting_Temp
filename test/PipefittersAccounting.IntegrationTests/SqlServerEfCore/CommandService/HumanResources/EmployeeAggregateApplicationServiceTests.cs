#pragma warning disable CS8600
#pragma warning disable CS8602

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
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();

            OperationResult<bool> result = await _appService.CreateEmployeeInfo(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateEmployeeInfo_EmployeeAggregateApplicationService_EmployeeAlreadyExists_ShouldFail()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();

            OperationResult<bool> result = await _appService.CreateEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateEmployeeInfo_EmployeeAggregateApplicationService_DuplicateName_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            OperationResult<bool> result = await _appService.CreateEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task EditEmployeeInfo_EmployeeAggregateApplicationService_ShouldSucceed()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();

            OperationResult<bool> result = await _appService.EditEmployeeInfo(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task EditEmployeeInfo_EmployeeAggregateApplicationService_InvalidEmployeeID_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            model.EmployeeId = System.Guid.NewGuid();

            OperationResult<bool> result = await _appService.EditEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task EditEmployeeInfo_EmployeeAggregateApplicationService_DuplicateName_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            OperationResult<bool> result = await _appService.EditEmployeeInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteEmployeeInfo_EmployeeAggregateApplicationService_HasTimeCards_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();

            OperationResult<bool> result = await _appService.DeleteEmployeeInfo(model);

            Assert.False(result.Success);
        }
    }
}