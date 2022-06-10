using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;


namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class StockSubscriptionBusinessRulesTests : TestBaseDapper
    {
        private readonly IStockSubscriptionQueryService _queryService;

        public StockSubscriptionBusinessRulesTests()
        {
            _queryService = new StockSubscriptionQueryService(_dapperCtx);
        }

        [Fact]
        public async Task Validate_VerifyStockSubscriptionIdentificationRule_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            VerifyStockSubscriptionStockIdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyInvestorIdentificationRule_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            VerifyInvestorIdentificationRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyStockSubscriptionIsUniqueRule_DuplicateSameStockId_ShouldSucceed()
        {
            // Existing with the same StockId
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            VerifyStockSubscriptionIsUniqueRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyStockSubscriptionIsUniqueRule_DifferentFieldValues_ShouldSucceed()
        {
            // No existing subscription has the same financierId, stockIssueDate, sharesIssued, pricePerShare combination
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            VerifyStockSubscriptionIsUniqueRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyStockSubscriptionIsUniqueRule__DuplicateDifferentStockId_ShouldFail()
        {
            // Existing with a different StockId, saving this would result in a duplicate
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            model.StockIssueDate = new DateTime(2022, 4, 2);
            model.SharesIssued = 12500;
            model.PricePerShare = 1.25M;

            VerifyStockSubscriptionIsUniqueRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotEditOrDeleteIfStockIssueProceedsRcvdRule_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotEditOrDeleteIfStockIssueProceedsRcvdRule_DepositRcvd_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithDeposit();
            CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDividendDeclarationStockIdRule_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();

            VerifyDividendDeclarationStockIdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDividendDeclarationStockIdRule_InvalidStockId_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            model.StockId = new Guid("1c967462-140a-4e08-9ba2-04ff760bb1d9");

            VerifyDividendDeclarationStockIdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotAddDividendUntilStockIssueProceedsHaveBeenRcvdRule_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();

            CannotAddDividendUntilStockIssueProceedsHaveBeenRcvdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotAddDividendUntilStockIssueProceedsHaveBeenRcvdRule_NotRcvd_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            model.StockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354");

            CannotAddDividendUntilStockIssueProceedsHaveBeenRcvdRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotEditDeleteDividendDeclarationIfPaidRule_NotPaid_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();

            CannotEditDeleteDividendDeclarationIfPaidRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CannotEditDeleteDividendDeclarationIfPaidRule_Paid_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditPaid();

            CannotEditDeleteDividendDeclarationIfPaidRule rule = new(_queryService);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }
    }
}