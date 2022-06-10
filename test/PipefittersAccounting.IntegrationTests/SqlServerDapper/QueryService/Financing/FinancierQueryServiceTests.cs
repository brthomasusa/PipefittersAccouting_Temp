using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class FinancierQueryServiceTests : TestBaseDapper
    {
        private readonly IFinancierQueryService queryService;

        public FinancierQueryServiceTests() => queryService = new FinancierQueryService(_dapperCtx);


        [Fact]
        public async Task GetFinancierDetails_GetOneFinancierDetailById_ShouldSucceed()
        {
            Guid agentID = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");

            GetFinancier qryParam = new GetFinancier() { FinancierId = agentID };
            OperationResult<FinancierDetail> result = await queryService.GetFinancierDetails(qryParam);

            Assert.True(result.Success);
            Assert.Equal(agentID, result.Result.FinancierId);
            Assert.Equal("Bertha Mae Jones Innovative Financing", result.Result.FinancierName);
        }

        [Fact]
        public async Task GetFinancierDetails_GetFinancierDetailWithInvalidId_ShouldFail()
        {
            Guid agentID = new Guid("aaa471a0-5c1e-4a4d-97e7-288fb0f6338a");

            GetFinancier qryParam = new GetFinancier() { FinancierId = agentID };
            OperationResult<FinancierDetail> result = await queryService.GetFinancierDetails(qryParam);

            Assert.False(result.Success);
            Assert.Equal($"No financier record found where FinancierId equals {qryParam.FinancierId}.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetFinancierListItems_RetrievePageListOfFinancierListItems_ShouldSucceed()
        {
            // Get page 1 of 2
            GetFinanciers queryParameters = new GetFinanciers() { Page = 1, PageSize = 3 };
            OperationResult<PagedList<FinancierListItems>> result = await queryService.GetFinancierListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(3, result.Result.Count);

            // Get page 2 of 2
            queryParameters = new GetFinanciers() { Page = 2, PageSize = 3 };
            result = await queryService.GetFinancierListItems(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(3, result.Result.Count);
        }
    }
}