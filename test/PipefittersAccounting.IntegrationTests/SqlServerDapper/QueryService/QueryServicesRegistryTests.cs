using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService
{
    public class QueryServicesRegistryTests : TestBaseDapper
    {
        private readonly IFinancierQueryService _financierQueryService;
        private readonly IStockSubscriptionQueryService _stockSubscriptionQueryService;

        public QueryServicesRegistryTests()
        {
            _financierQueryService = new FinancierQueryService(_dapperCtx);
            _stockSubscriptionQueryService = new StockSubscriptionQueryService(_dapperCtx);
        }

        [Fact]
        public void Register_QueryServicesRegistry_FinancierQueryService()
        {
            IQueryServicesRegistry registry = new QueryServicesRegistry();
            registry.RegisterService("FinancierQueryService", _financierQueryService);
            var qrySvc = registry.GetService<FinancierQueryService>("FinancierQueryService");

            Assert.IsType<FinancierQueryService>(qrySvc);
        }

        [Fact]
        public void Register_QueryServicesRegistry_FinancierQueryService_StockSubscriptionQueryService()
        {
            IQueryServicesRegistry registry = new QueryServicesRegistry();
            registry.RegisterService("FinancierQueryService", _financierQueryService);
            registry.RegisterService("StockSubscriptionQueryService", _stockSubscriptionQueryService);

            var financierQrySvc = registry.GetService<FinancierQueryService>("FinancierQueryService");
            var subscriptionQrySvc = registry.GetService<StockSubscriptionQueryService>("StockSubscriptionQueryService");

            Assert.IsType<FinancierQueryService>(financierQrySvc);
            Assert.IsType<StockSubscriptionQueryService>(subscriptionQrySvc);
        }
    }
}