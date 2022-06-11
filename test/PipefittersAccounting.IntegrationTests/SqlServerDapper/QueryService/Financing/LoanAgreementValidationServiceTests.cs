using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;
using PipefittersAccounting.Infrastructure.Interfaces;



namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    public class LoanAgreementValidationServiceTests : TestBaseDapper
    {
        private IQueryServicesRegistry _registry;

        public LoanAgreementValidationServiceTests()
        {
            ILoanAgreementQueryService _loanAgreementQueryService = new LoanAgreementQueryService(_dapperCtx);
            ISharedQueryService _sharedQueryService = new SharedQueryService(_dapperCtx);

            _registry = new QueryServicesRegistry();
            _registry.RegisterService("LoanAgreementQueryService", _loanAgreementQueryService);
            _registry.RegisterService("SharedQueryService", _sharedQueryService);
        }

        /*                               Business Rules                                     */

        [Fact]
        public async Task Validate_VerifyAgentIsFinancierRule_ShouldReturnTrue()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            VerifyAgentIsFinancierRule rule = new(_registry.GetService<SharedQueryService>("SharedQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyAgentIsFinancierRule_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            VerifyAgentIsFinancierRule rule = new(_registry.GetService<SharedQueryService>("SharedQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyLoanAgreementIsNotDuplicateRule_ShouldReturnTrue()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();

            VerifyAgentIsFinancierRule rule = new(_registry.GetService<SharedQueryService>("SharedQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyLoanAgreementIsNotDuplicateRule_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            model.LoanAmount = 30000M;
            model.InterestRate = 0.0863M;
            model.LoanDate = new DateTime(2022, 2, 2);
            model.MaturityDate = new DateTime(2024, 2, 2);

            VerifyLoanAgreementIsNotDuplicateRule rule = new(_registry.GetService<LoanAgreementQueryService>("LoanAgreementQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyCreditorIsLinkedToLoanAgreementRule_ShouldReturnTrue()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithDeposit();

            VerifyCreditorIsLinkedToLoanAgreementRule rule = new(_registry.GetService<LoanAgreementQueryService>("LoanAgreementQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyCreditorIsLinkedToLoanAgreementRule_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithDeposit();
            model.FinancierId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");

            VerifyCreditorIsLinkedToLoanAgreementRule rule = new(_registry.GetService<LoanAgreementQueryService>("LoanAgreementQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyLoanProceedsNotReceivedRule_WithoutDeposit_ShouldReturnTrue()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();

            VerifyLoanProceedsNotReceivedRule rule = new(_registry.GetService<LoanAgreementQueryService>("LoanAgreementQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyLoanProceedsNotReceivedRule_WithDeposit_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithDeposit();

            VerifyLoanProceedsNotReceivedRule rule = new(_registry.GetService<LoanAgreementQueryService>("LoanAgreementQueryService"));

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        /*                               Validators                                     */

        [Fact]
        public async Task Validate_CreateLoanAgreementValidator_ShouldSucceed()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            CreateLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateLoanAgreementValidator_InvalidCreditor_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            CreateLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_Validate_CreateLoanAgreementValidator_Duplicate_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();
            model.FinancierId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            model.LoanAmount = 30000M;
            model.InterestRate = 0.0863M;
            model.LoanDate = new DateTime(2022, 2, 2);
            model.MaturityDate = new DateTime(2024, 2, 2);

            CreateLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditLoanAgreementValidator_ShouldSucceed()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            EditLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditLoanAgreementValidator_InvalidCreditor_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            model.FinancierId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            EditLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditLoanAgreementValidator_InvalidLoanAgreement_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            model.LoanId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            EditLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditLoanAgreementValidator_CreditorNotLinkedToLoanAgreement_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithOutDeposit();
            model.LoanId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            EditLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditLoanAgreementValidator_LoanProceedsHaveBeenDeposited_ShouldReturnFalse()
        {
            LoanAgreementWriteModel model = LoanAgreementTestData.GetEditLoanAgreementInfoWithDeposit();

            EditLoanAgreementValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }
    }
}