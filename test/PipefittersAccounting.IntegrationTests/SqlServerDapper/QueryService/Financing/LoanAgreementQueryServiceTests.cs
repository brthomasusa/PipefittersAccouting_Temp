#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
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
        public async Task GetLoanAgreementDetails_ShouldSucceed()
        {
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            GetLoanAgreement qryParam = new GetLoanAgreement() { LoanId = loanID };
            OperationResult<LoanAgreementDetail> result = await _queryService.GetLoanAgreementDetails(qryParam);

            Assert.True(result.Success);
            int count = result.Result.LoanInstallmentListItems.Count;
            Assert.Equal(24, count);
        }

        [Fact]
        public async Task GetLoanAgreementDetails_WithInvalidLoanId_ShouldFail()
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
        public async Task GetLoanIdOfDuplicationLoanAgreement_ShouldReturnGuid()
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
        public async Task GetLoanIdOfDuplicationLoanAgreement_ShouldReturnEmptyGuid()
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
    }
}