using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    public class FinancierQueryServiceDapperTests : TestBaseDapper
    {
        [Fact]
        public async Task GetFinancierDetails_GetOneFinancierDetailById_ShouldSucceed()
        {
            IFinancierQueryService queryService = new FinancierQueryServiceDapper(_dapperCtx);

            Guid agentID = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");

            GetFinancier qryParam = new GetFinancier() { FinancierId = agentID };
            OperationResult<FinancierDetail> result = await queryService.GetFinancierDetails(qryParam);

            Assert.True(result.Success);
            Assert.Equal(agentID, result.Result.FinancierId);
            Assert.Equal("Bertha Mae Jones Innovative Financing", result.Result.FinancierName);
        }

        [Fact]
        public async Task GetFinancierListItems_RetrievePageListOfFinancierListItems_ShouldSucceed()
        {
            IFinancierQueryService queryService = new FinancierQueryServiceDapper(_dapperCtx);

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