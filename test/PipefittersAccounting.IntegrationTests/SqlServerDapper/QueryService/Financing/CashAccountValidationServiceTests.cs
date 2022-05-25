using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashAccountValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashAccountValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_queryService);
        }

        /*********************************************************************/
        /*                   Cash account validators                      */
        /*********************************************************************/

        [Fact]
        public void Create_NewCashAccountNameMustBeUniqueValidator_ShouldSucceed()
        {
            var exception = Record.Exception(() => new NewCashAccountNameMustBeUniqueRule(_queryService));
            Assert.Null(exception);
        }

        [Fact]
        public void Create_NewCashAccountNumberMustBeUniqueValidator_ShouldSucceed()
        {
            var exception = Record.Exception(() => new NewCashAccountNumberMustBeUniqueRule(_queryService));
            Assert.Null(exception);
        }



        [Fact]
        public async Task Validate_NewCashAccountNameMustBeUniqueValidator_ValidAcctName_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_NewCashAccountNameMustBeUniqueValidator_ExistingAcctName_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_NewCashAccountNumberMustBeUniqueValidator_ValidAcctNumber_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);

            ValidationResult validationResult = await acctNumberValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_NewCashAccountNumberMustBeUniqueValidator_ExistingAcctNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountNumber = "36547-9098812";
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);

            ValidationResult validationResult = await acctNumberValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account number '{model.CashAccountNumber}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_ValidAcctNameAndNumber_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_InvalidAcctNameAndValidNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);
            model.CashAccountName = "Payroll";
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_InvalidAcctNameAndInvalidNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);
            model.CashAccountName = "Payroll";
            model.CashAccountNumber = "36547-9098812";
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_ValidAcctNameAndInvalidNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);
            model.CashAccountNumber = "36547-9098812";
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account number '{model.CashAccountNumber}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_EditedCashAccountNameMustBeUniqueValidator_ValidAcctName_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            EditedCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditedCashAccountNameMustBeUniqueValidator_ExistingAcctName_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Payroll";
            EditedCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        /*********************************************************************/
        /*                   Validation, not validators                      */
        /*********************************************************************/

        [Fact]
        public async Task Validate_CreateCashAccountInfoValidation_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            ValidationResult validationResult = await CreateCashAccountInfoValidation.Validate(model, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateCashAccountInfoValidation_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            ValidationResult validationResult = await CreateCashAccountInfoValidation.Validate(model, _queryService);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_EditCashAccountInfoValidation_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            ValidationResult validationResult = await EditCashAccountInfoValidation.Validate(model, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditCashAccountInfoValidation_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Financing Proceeds";
            ValidationResult validationResult = await EditCashAccountInfoValidation.Validate(model, _queryService);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        /*********************************************************************/
        /*                   Validation service                      */
        /*********************************************************************/

        [Fact]
        public async Task IsValidCreateCashAccountInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCreateCashAccountInfo_CashAccountAggregateValidationService_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_CannotChangeAcctType_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfoWithAcctTypeUpdate();
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Primary Checking";
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ChangeAcctTypeNoTrans_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountType = 3;
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }
    }
}