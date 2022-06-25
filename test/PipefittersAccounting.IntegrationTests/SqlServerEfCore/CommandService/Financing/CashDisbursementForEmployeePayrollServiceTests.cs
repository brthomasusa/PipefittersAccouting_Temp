#pragma warning disable CS8618

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;


namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    public class CashDisbursementForEmployeePayrollServiceTests : TestBaseEfCore
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly IEmployeeAggregateQueryService _employeeQrySvc;
        private readonly ISharedQueryService _sharedQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private IQueryServicesRegistry _registry;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeePayrollService _payrollService;
        private readonly ICashAccountApplicationService _appSvc;

        public CashDisbursementForEmployeePayrollServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _employeeQrySvc = new EmployeeAggregateQueryService(_dapperCtx);
            _sharedQrySvc = new SharedQueryService(_dapperCtx);
            _registry = new QueryServicesRegistry();
            _registry.RegisterService("CashAccountQueryService", _queryService);
            _registry.RegisterService("SharedQueryService", _sharedQrySvc);
            _validationService = new CashAccountAggregateValidationService(_queryService, _sharedQrySvc, _registry);
            _repository = new CashAccountAggregateRepository(_dbContext);
            _unitOfWork = new AppUnitOfWork(_dbContext);
            _payrollService = new EmployeePayrollService(_queryService, _employeeQrySvc, _validationService, _repository, _unitOfWork);
            _appSvc = new CashAccountApplicationService(_validationService, _repository, _payrollService, _unitOfWork);
        }


        // ICashAccountAggregateValidationService validationService,
        // ICashAccountAggregateRepository repo,
        // IEmployeePayrollService payrollService,
        // IUnitOfWork unitOfWork
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

        [Fact]
        public async Task Process_EmployeePayrollService_ShouldSucceed()
        {
            Guid cashAccountId = new Guid("c98ac84f-00bb-463d-9116-5828b2e9f718");
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<bool> result = await _payrollService.Process(cashAccountId, userId);
            if (!result.Success)
                Console.WriteLine(result.NonSuccessMessage);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateDisbursementsForPayroll_CashAccountApplicationServicee_ShouldSucceed()
        {
            Guid cashAccountId = new Guid("c98ac84f-00bb-463d-9116-5828b2e9f718");
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<bool> result = await _appSvc.CreateDisbursementsForPayroll(cashAccountId, userId);

            if (!result.Success)
                Console.WriteLine(result.NonSuccessMessage);

            Assert.False(result.Success);
        }
    }
}