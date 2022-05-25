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
    public class CashTransferValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashTransferValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_queryService);
        }

        [Fact]
        public async Task Validate_SourceCashAccountBalanceValidator_ShouldSucceed()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            SourceCashAccountBalanceRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_SourceCashAccountBalanceValidator_InsufficientBalance_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.CashTransferAmount = 20000.01M;

            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_SourceCashAccountValidator_ShouldSucceed()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_SourceCashAccountValidator_SourceSamesAsDestination_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = transferInfo.SourceCashAccountId;

            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_SourceCashAccountValidator_InvalidSourceAcctId_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.SourceCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            SourceCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DestinationCashAccountValidator_ShouldSucceed()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            DestinationCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DestinationCashAccountValidator_SourceSamesAsDestination_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = transferInfo.SourceCashAccountId;

            DestinationCashAccountRule validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(transferInfo);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DestinationCashAccountValidator_InvalidDestinationAcctId_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
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
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();

            ValidationResult validationResult = await CashAccountTransferValidation.Validate(transferInfo, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_SourceSamesAsDestination_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = transferInfo.SourceCashAccountId;

            ValidationResult validationResult = await CashAccountTransferValidation.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_InvalidDestinationAcctId_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.DestinationCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            ValidationResult validationResult = await CashAccountTransferValidation.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_InvalidSourceAcctId_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.SourceCashAccountId = new Guid("111bb318-649e-470d-9d2b-693bfb0b2744");

            ValidationResult validationResult = await CashAccountTransferValidation.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountTransferValidation_InsufficientBalance_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.CashTransferAmount = 20000.01M;

            ValidationResult validationResult = await CashAccountTransferValidation.Validate(transferInfo, _queryService);

            Assert.False(validationResult.IsValid);
        }

        /*********************************************************************/
        /*                   Validation service                              */
        /*********************************************************************/

        [Fact]
        public async Task IsValidCreateCashAccountTransferInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountTransferInfo(transferInfo);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCreateCashAccountTransferInfo_CashAccountAggregateValidationService_InsufficientBalance_ShouldFail()
        {
            CreateCashAccountTransferInfo transferInfo = CashAccountTestData.GetCreateCashAccountTransferInfo();
            transferInfo.CashTransferAmount = 20000.01M;

            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountTransferInfo(transferInfo);

            Assert.False(validationResult.IsValid);
        }






    }
}