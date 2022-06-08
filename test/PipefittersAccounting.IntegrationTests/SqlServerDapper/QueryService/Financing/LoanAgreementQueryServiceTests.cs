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
        [Fact]
        public async Task GetLoanAgreementDetails_ShouldSucceed()
        {
            ILoanAgreementQueryService queryService = new LoanAgreementQueryService(_dapperCtx);
            Guid loanID = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            GetLoanAgreement qryParam = new GetLoanAgreement() { LoanId = loanID };
            OperationResult<LoanAgreementDetail> result = await queryService.GetLoanAgreementDetails(qryParam);

            Assert.True(result.Success);
            int count = result.Result.LoanInstallmentListItems.Count;
            Assert.Equal(24, count);
        }

        [Fact]
        public async Task GetLoanAgreementDetails_WithInvalidLoanId_ShouldFail()
        {
            ILoanAgreementQueryService queryService = new LoanAgreementQueryService(_dapperCtx);
            Guid loanID = new Guid("93adf7e5-bf6c-4ec8-881a-bfdf37aaf12e");

            GetLoanAgreement qryParam = new GetLoanAgreement() { LoanId = loanID };
            OperationResult<LoanAgreementDetail> result = await queryService.GetLoanAgreementDetails(qryParam);

            Assert.False(result.Success);
            Assert.Equal($"Unable to locate a loan agreement with LoanId: {qryParam.LoanId}.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetLoanAgreementListItems_Retrieve_PageList_LoanAgreementListItem_ShouldSucceed()
        {
            ILoanAgreementQueryService queryService = new LoanAgreementQueryService(_dapperCtx);

            // Get page 1 of 2
            GetLoanAgreements queryParameters = new GetLoanAgreements { Page = 1, PageSize = 2 };
            OperationResult<PagedList<LoanAgreementListItem>> result = await queryService.GetLoanAgreementListItems(queryParameters);

            Assert.True(result.Success);
            Assert.True(result.Result.Count == 2);

            // Get page 2 of 2
            queryParameters = new GetLoanAgreements { Page = 2, PageSize = 2 };
            result = await queryService.GetLoanAgreementListItems(queryParameters);

            Assert.True(result.Success);
            Assert.True(result.Result.Count == 2);
        }
    }
}