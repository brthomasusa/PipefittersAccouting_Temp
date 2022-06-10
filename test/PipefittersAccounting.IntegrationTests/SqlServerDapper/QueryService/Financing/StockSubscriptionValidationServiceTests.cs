using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;
using PipefittersAccounting.Infrastructure.Interfaces;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    public class StockSubscriptionValidationServiceTests : TestBaseDapper
    {
        private IQueryServicesRegistry _registry;

        public StockSubscriptionValidationServiceTests()
        {
            IStockSubscriptionQueryService _queryService = new StockSubscriptionQueryService(_dapperCtx);

            _registry = new QueryServicesRegistry();
            _registry.RegisterService("StockSubscriptionQueryService", _queryService);
        }

        [Fact]
        public async Task Validate_CreateDividendDeclarationValidation_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            CreateDividendDeclarationValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateDividendDeclarationValidation_InvalidStockId_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            model.StockId = new Guid("1c967462-140a-4e08-9ba2-04ff760bb1d9");

            CreateDividendDeclarationValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateDividendDeclarationValidation_ProceedsRcvd_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            CreateDividendDeclarationValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateDividendDeclarationValidation_ProceedsNotRcvd_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            model.StockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354");

            CreateDividendDeclarationValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditDividendDeclarationValidation_NotPaid_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();
            CreateDividendDeclarationValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditDividendDeclarationValidation_Paid_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditPaid();
            EditDividendDeclarationValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        /*********************************************************************/
        /*                        Stock subscription                         */
        /*********************************************************************/

        [Fact]
        public async Task Validate_CreateStockSubscriptionValidation_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            CreateStockSubscriptionValidator validator = new(model, _registry);

            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateStockSubscriptionValidation__InvalidFinancierId_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("bffbcf34-f6ba-4fb2-b70e-ab19d3371886");

            CreateStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateStockSubscriptionValidation__Duplicate_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            model.StockIssueDate = new DateTime(2022, 4, 2);
            model.SharesIssued = 12500;
            model.PricePerShare = 1.25M;

            CreateStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditStockSubscriptionValidation_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            EditStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditStockSubscriptionValidation_InvalidStockId_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.StockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            EditStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditStockSubscriptionValidation_InvalidFinancierIdCombo_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12353ffb-9983-4cde-b1d6-8a49e785177f");

            EditStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditStockSubscriptionValidation_InvalidStockIdFinancierIdCombo_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("bffbcf34-f6ba-4fb2-b70e-ab19d3371886");

            EditStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditStockSubscriptionValidation_Duplicate_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            model.StockIssueDate = new DateTime(2022, 4, 2);
            model.SharesIssued = 12500;
            model.PricePerShare = 1.25M;

            EditStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditStockSubscriptionValidation_ExistingDeposit_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithDeposit();

            EditStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DeleteStockSubscriptionValidation_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();

            DeleteStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DeleteStockSubscriptionValidation_InvalidStockId_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.StockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            DeleteStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DeleteStockSubscriptionValidation_ExistingDeposit_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithDeposit();

            DeleteStockSubscriptionValidator validator = new(model, _registry);
            ValidationResult validationResult = await validator.Validate();

            Assert.False(validationResult.IsValid);
        }
    }
}