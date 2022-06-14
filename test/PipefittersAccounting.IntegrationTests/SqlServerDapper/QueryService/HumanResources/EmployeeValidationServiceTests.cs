using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.IntegrationTests.Base;
using PipefittersAccounting.Infrastructure.Interfaces;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.HumanResources
{
    public class EmployeeValidationServiceTests : TestBaseDapper
    {
        private readonly IEmployeeAggregateQueryService _employeeQueryService;
        private readonly ISharedQueryService _sharedQueryService;
        private IQueryServicesRegistry _registry;

        public EmployeeValidationServiceTests()
        {
            _employeeQueryService = new EmployeeAggregateQueryService(_dapperCtx);
            _sharedQueryService = new SharedQueryService(_dapperCtx);

            _registry = new QueryServicesRegistry();
            _registry.RegisterService("EmployeeAggregateQueryService", _employeeQueryService);
            _registry.RegisterService("SharedQueryService", _sharedQueryService);
        }

        /*                               Business Rules                                     */

        [Fact]
        public async Task Validate_VerifyAgentIsEmployeeRule_ShouldReturnTrue()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            VerifyAgentIsEmployeeRule rule = new(_sharedQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyAgentIsEmployeeRule_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();
            VerifyAgentIsEmployeeRule rule = new(_sharedQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEmployeeNameIsUniqueRule_Create_ShouldReturnTrue()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();
            VerifyEmployeeNameIsUniqueRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEmployeeNameIsUniqueRule_Create_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            VerifyEmployeeNameIsUniqueRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEmployeeNameIsUniqueRule_Edit_ShouldReturnTrue()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            VerifyEmployeeNameIsUniqueRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEmployeeNameIsUniqueRule_Edit_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            VerifyEmployeeNameIsUniqueRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotDeleteEmployeeIfTimeCardExistRule_Has2TimeCards_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            CannotDeleteEmployeeIfTimeCardExistRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }


        /*                               Validators                                     */

























    }
}