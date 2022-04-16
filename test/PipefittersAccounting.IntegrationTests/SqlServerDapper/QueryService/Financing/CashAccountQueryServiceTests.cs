using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashAccountQueryServiceTests : TestBaseDapper
    {
        [Fact]
        public async Task GetFinancierIdValidationModel_WithValidId_ShouldSucceed()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid financierID = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");

            FinancierIdValidationParams qryParam = new() { FinancierId = financierID };
            OperationResult<FinancierIdValidationModel> result = await queryService.GetFinancierIdValidationModel(qryParam);

            Assert.True(result.Success);
            Assert.Equal("Bertha Mae Jones Innovative Financing", result.Result.FinancierName);
        }

        [Fact]
        public async Task GetFinancierIdValidationModel_WithInvalidId_ShouldFail()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid financierID = new Guid("76e6164a-249d-47a2-b47c-f09a332181b6");

            FinancierIdValidationParams qryParam = new() { FinancierId = financierID };
            OperationResult<FinancierIdValidationModel> result = await queryService.GetFinancierIdValidationModel(qryParam);

            Assert.False(result.Success);
            Assert.Equal($"Unable to locate a financier with FinancierId: {qryParam.FinancierId}.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetReceiptLoanProceedsValidationModel_WithValidId_ShouldSucceed()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid financierID = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ReceiptLoanProceedsValidationParams qryParam = new() { FinancierId = financierID, LoanId = loanID };
            OperationResult<ReceiptLoanProceedsValidationModel> result =
                await queryService.GetReceiptLoanProceedsValidationModel(qryParam);

            Assert.True(result.Success);
            Assert.Equal("Paul Van Horn Enterprises", result.Result.FinancierName);
            Assert.Equal(30000M, result.Result.LoanAmount);
            Assert.Equal(new DateTime(2022, 3, 19), result.Result.DateReceived);
            Assert.Equal(30000M, result.Result.AmountReceived);
        }

        [Fact]
        public async Task GetDisburesementLoanPymtValidationModel_WithValidIdLoanInstallmentId_ShouldSucceed()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid loanInstallmentID = new Guid("76e6164a-249d-47a2-b47c-f09a332181b6");

            DisburesementLoanPymtValidationParams qryParam = new() { LoanInstallmentId = loanInstallmentID };
            OperationResult<DisburesementLoanPymtValidationModel> result =
                await queryService.GetDisburesementLoanPymtValidationModel(qryParam);

            Assert.True(result.Success);
            Assert.Equal(new Guid("12998229-7ede-4834-825a-0c55bde75695"), result.Result.FinancierId);
            Assert.Equal(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"), result.Result.LoanId);
        }



        [Fact]
        public async Task GetCreditorHasLoanAgreeValidationModel_ValidFinancierIdLoanId_ShouldSucceed()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid financierID = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid loanID = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");

            CreditorHasLoanAgreeValidationParams qryParam = new() { FinancierId = financierID, LoanId = loanID };

            OperationResult<CreditorHasLoanAgreeValidationModel> result =
                await queryService.GetCreditorHasLoanAgreeValidationModel(qryParam);

            Assert.True(result.Success);
            Assert.Equal("Arturo Sandoval", result.Result.FinancierName);
            Assert.Equal(25000M, result.Result.LoanAmount);
        }

        [Fact]
        public async Task GetCreditorHasLoanAgreeValidationModel_InValidLoanId_ShouldFail()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid financierID = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid loanID = new Guid("1511c20b-6df0-4313-98a5-7c3561757dc2");

            CreditorHasLoanAgreeValidationParams qryParam = new() { FinancierId = financierID, LoanId = loanID };

            OperationResult<CreditorHasLoanAgreeValidationModel> result =
                await queryService.GetCreditorHasLoanAgreeValidationModel(qryParam);

            Assert.False(result.Success);
            Assert.Equal($"Unable to locate a loan agreement with loan Id: {qryParam.LoanId} for financier: {qryParam.FinancierId}.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetCreditorHasLoanAgreeValidationModel_InValidFinancierId_ShouldFail()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            Guid financierID = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");
            Guid loanID = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");

            CreditorHasLoanAgreeValidationParams qryParam = new() { FinancierId = financierID, LoanId = loanID };

            OperationResult<CreditorHasLoanAgreeValidationModel> result =
                await queryService.GetCreditorHasLoanAgreeValidationModel(qryParam);

            Assert.False(result.Success);
            Assert.Equal($"Unable to locate a loan agreement with loan Id: {qryParam.LoanId} for financier: {qryParam.FinancierId}.", result.NonSuccessMessage);
        }
    }
}