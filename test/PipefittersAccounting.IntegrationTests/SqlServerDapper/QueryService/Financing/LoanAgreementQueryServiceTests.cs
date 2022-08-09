#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class LoanAgreementQueryServiceTests : TestBaseDapper
    {
        private readonly ILoanAgreementQueryService _queryService;

        public LoanAgreementQueryServiceTests() =>
            _queryService = new LoanAgreementQueryService(_dapperCtx);

        [Fact]
        public async Task GetLoanAgreementDetails_LoanAgreementQueryService_ShouldSucceed()
        {
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            GetLoanAgreement qryParam = new GetLoanAgreement() { LoanId = loanID };
            OperationResult<LoanAgreementDetail> result = await _queryService.GetLoanAgreementDetails(qryParam);

            Assert.True(result.Success);
            int count = result.Result.LoanInstallmentListItems.Count;
            Assert.Equal(24, count);
        }

        [Fact]
        public async Task GetLoanAgreementDetails_LoanAgreementQueryService_WithInvalidLoanId_ShouldFail()
        {
            Guid loanID = new Guid("93adf7e5-bf6c-4ec8-881a-bfdf37aaf12e");

            GetLoanAgreement qryParam = new GetLoanAgreement() { LoanId = loanID };
            OperationResult<LoanAgreementDetail> result = await _queryService.GetLoanAgreementDetails(qryParam);

            Assert.False(result.Success);
            Assert.Equal($"Unable to locate a loan agreement with LoanId: {qryParam.LoanId}.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetLoanAgreementListItems_Retrieve_PageList_LoanAgreementListItem_ShouldSucceed()
        {
            // Get page 1 of 2
            GetLoanAgreements queryParameters = new GetLoanAgreements { Page = 1, PageSize = 2 };
            OperationResult<PagedList<LoanAgreementListItem>> result = await _queryService.GetLoanAgreementListItems(queryParameters);

            Assert.True(result.Success);
            Assert.True(result.Result.Count == 2);

            // Get page 2 of 2
            queryParameters = new GetLoanAgreements { Page = 2, PageSize = 2 };
            result = await _queryService.GetLoanAgreementListItems(queryParameters);

            Assert.True(result.Success);
            Assert.True(result.Result.Count == 2);
        }

        [Fact]
        public async Task GetLoanIdOfDuplicationLoanAgreement_LoanAgreementQueryService_ShouldReturnGuid()
        {
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            Guid financierID = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            decimal loanAmount = 30000M;
            decimal interestRate = 0.0863M;
            DateTime loanDate = new DateTime(2022, 2, 2);
            DateTime maturityDate = new DateTime(2024, 2, 2);

            GetDuplicateLoanAgreement qryParam = new GetDuplicateLoanAgreement()
            {
                FinancierId = financierID,
                LoanAmount = loanAmount,
                InterestRate = interestRate,
                LoanDate = loanDate,
                MaturityDate = maturityDate
            };

            OperationResult<Guid> result = await _queryService.GetLoanIdOfDuplicationLoanAgreement(qryParam);

            Assert.True(result.Success);

            Assert.Equal(loanID, result.Result);
        }

        [Fact]
        public async Task GetLoanIdOfDuplicationLoanAgreement_LoanAgreementQueryService_ShouldReturnEmptyGuid()
        {
            Guid financierID = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            decimal loanAmount = 30000M;
            decimal interestRate = 0.0863M;
            DateTime loanDate = new DateTime(2022, 2, 2);
            DateTime maturityDate = new DateTime(2024, 2, 2);

            GetDuplicateLoanAgreement qryParam = new GetDuplicateLoanAgreement()
            {
                FinancierId = financierID,
                LoanAmount = loanAmount,
                InterestRate = interestRate,
                LoanDate = loanDate,
                MaturityDate = maturityDate
            };

            OperationResult<Guid> result = await _queryService.GetLoanIdOfDuplicationLoanAgreement(qryParam);

            Assert.True(result.Success);

            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task VerifyCashDepositForDebtIssueProceeds_LoanAgreementQueryService_Return_0()
        {
            Guid financierID = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");
            Guid loanID = new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867");

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = financierID, LoanId = loanID };
            OperationResult<decimal> result = await _queryService.VerifyCashDepositForDebtIssueProceeds(qryParam);

            Assert.True(result.Success);
            Assert.Equal(0, result.Result);
        }

        [Fact]
        public async Task VerifyCashDepositForDebtIssueProceeds_LoanAgreementQueryService_Return_30000()
        {
            Guid financierID = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = financierID, LoanId = loanID };
            OperationResult<decimal> result = await _queryService.VerifyCashDepositForDebtIssueProceeds(qryParam);

            Assert.True(result.Success);
            Assert.Equal(30000M, result.Result);
        }

        [Fact]
        public async Task VerifyCashDepositForDebtIssueProceeds_LoanAgreementQueryService_InvalidCombo_Return_0()
        {
            // Both are valid, but the combonation is not.
            Guid financierID = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            Guid loanID = new Guid("1511c20b-6df0-4313-98a5-7c3561757dc2");

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = financierID, LoanId = loanID };
            OperationResult<decimal> result = await _queryService.VerifyCashDepositForDebtIssueProceeds(qryParam);

            Assert.True(result.Success);
            Assert.Equal(0, result.Result);
        }

        [Fact]
        public async Task VerifyCreditorIsLinkedToLoanAgreement_LoanAgreementQueryService_ReturnGuid()
        {
            Guid financierID = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = financierID, LoanId = loanID };
            OperationResult<Guid> result = await _queryService.VerifyCreditorIsLinkedToLoanAgreement(qryParam);

            Assert.True(result.Success);
            Assert.Equal(financierID, result.Result);
        }

        [Fact]
        public async Task VerifyCreditorIsLinkedToLoanAgreement_LoanAgreementQueryService_ReturnEmptyGuid()
        {
            // Both are valid, but the combonation is not.
            Guid financierID = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");
            Guid loanID = new Guid("1511c20b-6df0-4313-98a5-7c3561757dc2");

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = financierID, LoanId = loanID };
            OperationResult<Guid> result = await _queryService.VerifyCreditorIsLinkedToLoanAgreement(qryParam);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }
    }
}