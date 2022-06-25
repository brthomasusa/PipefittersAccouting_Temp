using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Shared;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    public class CashDisbursementForEmployeePayrollServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;

        public CashDisbursementForEmployeePayrollServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
        }

        [Fact]
        public async Task CreatePayrollCashTransactions_CreatePayrollCashTransactions_ShouldSucceed()
        {
            GetTimeCardPaymentInfoParameter queryParameters =
                new()
                {
                    PayPeriodBegin = new DateTime(2022, 2, 1),
                    PayPeriodEnd = new DateTime(2022, 2, 28)
                };

            OperationResult<List<TimeCardPaymentInfo>> result = await _queryService.GetTimeCardPaymentInfo(queryParameters);

            Assert.True(result.Success);

            CreatePayrollCashTransactions createPayrollCashTransactions
                = new
                (
                    new Guid("c98ac84f-00bb-463d-9116-5828b2e9f718"),
                    new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"),
                    result.Result
                );

            Assert.Equal(13, createPayrollCashTransactions.CashTransactions.Count);
        }
    }
}