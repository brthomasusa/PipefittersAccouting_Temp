using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashAccountValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ISharedQueryService _sharedQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;
        private IQueryServicesRegistry _registry;

        public CashAccountValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _sharedQrySvc = new SharedQueryService(_dapperCtx);
            _registry = new QueryServicesRegistry();
            _registry.RegisterService("CashAccountQueryService", _queryService);
            _registry.RegisterService("SharedQueryService", _sharedQrySvc);

            _validationService = new CashAccountAggregateValidationService(_queryService, _sharedQrySvc, _registry);
        }

        /*********************************************************************/
        /*                   Cash account business rules                     */
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
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_NewCashAccountNameMustBeUniqueValidator_ExistingAcctName_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
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
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);

            ValidationResult validationResult = await acctNumberValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_NewCashAccountNumberMustBeUniqueValidator_ExistingAcctNumber_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
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
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);
            NewCashAccountNumberMustBeUniqueRule acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_InvalidAcctNameAndValidNumber_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
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
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
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
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
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
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            EditedCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditedCashAccountNameMustBeUniqueValidator_ExistingAcctName_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Payroll";
            EditedCashAccountNameMustBeUniqueRule acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }




        /*********************************************************************/
        /*                       Business rule chains                        */
        /*********************************************************************/

        [Fact]
        public async Task Validate_CreateCashAccountInfoValidation_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            ValidationResult validationResult = await CreateCashAccountInfoValidator.Validate(model, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateCashAccountInfoValidation_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            ValidationResult validationResult = await CreateCashAccountInfoValidator.Validate(model, _queryService);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_EditCashAccountInfoValidation_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            ValidationResult validationResult = await EditCashAccountInfoValidator.Validate(model, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditCashAccountInfoValidation_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Financing Proceeds";
            ValidationResult validationResult = await EditCashAccountInfoValidator.Validate(model, _queryService);

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
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCreateCashAccountInfo_CashAccountAggregateValidationService_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_CannotChangeAcctType_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfoWithAcctTypeUpdate();
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Primary Checking";
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ChangeAcctTypeNoTrans_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountType = 3;
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }
    }
}