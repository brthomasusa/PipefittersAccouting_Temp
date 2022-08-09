using System;
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
    public class CashTransferValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ISharedQueryService _sharedQryService;
        private readonly ICashAccountAggregateValidationService _validationService;
        private IQueryServicesRegistry _registry;

        public CashTransferValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _sharedQryService = new SharedQueryService(_dapperCtx);

            _registry = new QueryServicesRegistry();
            _registry.RegisterService("CashAccountQueryService", _queryService);
            _registry.RegisterService("SharedQueryService", _sharedQryService);
            _validationService = new CashAccountAggregateValidationService(_queryService, _sharedQryService, _registry);
        }

        [Fact]
        public async Task Validate_SourceCashAccountBalanceValidator_ShouldSucceed()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            SourceCashAccountBalanceRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        // [Fact]
        // public async Task Validate_SourceCashAccountBalanceValidator_InsufficientBalance_ShouldFail()
        // {
        //     CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
        //     transferInfo.CashTransferAmount = 120000.01M;

        //     SourceCashAccountBalanceRule validator = new(_queryService);

        //     ValidationResult validationResult = await validator.Validate(transferInfo);

        //     Assert.False(validationResult.IsValid);
        // }

        [Fact]
        public async Task Validate_SourceCashAccountRule_ShouldSucceed()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_SourceCashAccountValidator_SourceSamesAsDestination_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = transferInfo.SourceCashAccountId;

            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_SourceCashAccountValidator_InvalidSourceAcctId_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.SourceCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DestinationCashAccountValidator_ShouldSucceed()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            DestinationCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DestinationCashAccountValidator_SourceSamesAsDestination_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = transferInfo.SourceCashAccountId;

            DestinationCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DestinationCashAccountValidator_InvalidDestinationAcctId_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            DestinationCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        /***********************************************************************************************************/
        /*                   Validation, not validators  A validation is a chain of validators                     */
        /***********************************************************************************************************/

        [Fact]
        public async Task Validate_CashAccountTransferValidation_ShouldSucceed()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();

            ValidationResult validationResult = await CashAccountTransferValidator.Validate(transferInfo, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_SourceSamesAsDestination_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = transferInfo.SourceCashAccountId;

            ValidationResult validationResult = await CashAccountTransferValidator.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_InvalidDestinationAcctId_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            ValidationResult validationResult = await CashAccountTransferValidator.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_InvalidSourceAcctId_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.SourceCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            ValidationResult validationResult = await CashAccountTransferValidator.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_InsufficientBalance_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.CashTransferAmount = 35625.01M;

            ValidationResult validationResult = await CashAccountTransferValidator.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        /*********************************************************************/
        /*                   Validation service                              */
        /*********************************************************************/

        [Fact]
        public async Task IsValidCreateCashAccountTransferInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountTransferInfo(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCreateCashAccountTransferInfo_CashAccountAggregateValidationService_InsufficientBalance_ShouldFail()
        {
            CashAccountTransferWriteModel transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.CashTransferAmount = 35625.01M;

            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountTransferInfo(transferInfo);

            Assert.False(validationResult.IsValid);
        }






    }
}