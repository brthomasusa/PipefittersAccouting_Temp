using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules;
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

        [Fact]
        public async Task Validate_VerifyPayPeriodEndedDateIsMostRecentRule_Has2TimeCards_ShouldReturnTrue()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            VerifyPayPeriodEndedDateIsMostRecentRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyPayPeriodEndedDateIsMostRecentRule_Has2TimeCards_DuplicateDate_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.PayPeriodEnded = new System.DateTime(2022, 2, 28);    // Invalid; date from previous pay period

            VerifyPayPeriodEndedDateIsMostRecentRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyPayPeriodEndedDateIsMostRecentRule_Has2TimeCards_NotEndOfMonth_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.PayPeriodEnded = new System.DateTime(2022, 3, 28);    // Invalid; date from previous pay period

            VerifyPayPeriodEndedDateIsMostRecentRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEmployeeSupervisorLinkRule_ShouldReturnTrue()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();

            VerifyEmployeeSupervisorLinkRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEmployeeSupervisorLinkRule_InvaldiSupvID_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.SupervisorId = new Guid("5c60f693-bef5-e011-a485-80ee7300c695");

            VerifyEmployeeSupervisorLinkRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyTimeCardPaymentRule_NotPaid_ShouldReturnTrue()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForEdit();

            VerifyTimeCardPaymentRule rule = new(_employeeQueryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        /*                               Validators                                     */

        [Fact]
        public async Task Validate_CreateEmployeeValidator_ShouldSucceed()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();
            CreateEmployeeValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateEmployeeValidator_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelCreate();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";
            CreateEmployeeValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate(); ;

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditEmployeeValidator_ShouldSucceed()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            EditEmployeeValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditEmployeeValidator_InvalidEmployeeID_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            model.EmployeeId = System.Guid.NewGuid();

            EditEmployeeValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditEmployeeValidator_DuplicateName_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();
            model.LastName = "Erickson";
            model.FirstName = "Gail";
            model.MiddleInitial = "A";

            EditEmployeeValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DeleteEmployeeValidator_HasTimeCards_ShouldReturnFalse()
        {
            EmployeeWriteModel model = TestUtilities.GetEmployeeWriteModelEdit();

            DeleteEmployeeValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateTimeCardValidator_ShouldSucceed()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            CreateTimeCardValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateTimeCardValidator_InvaldiEmpoyeeID_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.EmployeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53");

            CreateTimeCardValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateTimeCardValidator_InvaldiSupvID_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.SupervisorId = new Guid("5c60f693-bef5-e011-a485-80ee7300c695");

            CreateTimeCardValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateTimeCardValidator_DuplicateDate_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.PayPeriodEnded = new System.DateTime(2022, 2, 28);    // Invalid; date from previous pay period

            CreateTimeCardValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateTimeCardValidator_NotEndOfMonth_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForCreate();
            model.PayPeriodEnded = new System.DateTime(2022, 3, 28);    // Invalid; date from previous pay period

            CreateTimeCardValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditTimeCardValidator_ShouldSucceed()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForEdit();
            EditTimeCardValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditTimeCardValidator_InvalidEmployeeId_ShouldReturnFalse()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForEdit();
            model.EmployeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53");

            EditTimeCardValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DeleteTimeCardValidator_ShouldSucceed()
        {
            TimeCardWriteModel model = TestUtilities.GetTimeCardForEdit();
            DeleteTimeCardValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }
    }
}