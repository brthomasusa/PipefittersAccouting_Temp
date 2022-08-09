#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using Xunit;

using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.CashManagement;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.CashManagement;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    public class CashAccountAggregateApplicationServiceTests : TestBaseEfCore
    {
        private readonly ICashAccountApplicationService _appService;
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private readonly IEmployeePayrollService _payrollService;
        private readonly AppUnitOfWork _unitOfWork;
        private IQueryServicesRegistry _registry;

        public CashAccountAggregateApplicationServiceTests()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            IEmployeeAggregateQueryService employeeQrySvc = new EmployeeAggregateQueryService(_dapperCtx);
            ISharedQueryService sharedQueryService = new SharedQueryService(_dapperCtx);
            _repository = new CashAccountAggregateRepository(_dbContext);

            _registry = new QueryServicesRegistry();
            _registry.RegisterService("CashAccountQueryService", queryService);
            _registry.RegisterService("SharedQueryService", sharedQueryService);

            _validationService = new CashAccountAggregateValidationService(queryService, sharedQueryService, _registry);
            _unitOfWork = new AppUnitOfWork(_dbContext);
            _payrollService = new EmployeePayrollService(queryService, employeeQrySvc, _validationService, _repository, _unitOfWork);
            _appService = new CashAccountApplicationService(_validationService, _repository, _payrollService, _unitOfWork);
        }

        [Fact]
        public async void Process_CashAccountCreateCommand_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();

            OperationResult<bool> result =
                await CashAccountCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CashAccountCreateCommand_WithExistingAcctNumber_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountNumber = "36547-9098812";
            OperationResult<bool> result =
                await CashAccountCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
            string msg = $"There is an existing cash account with account number '{model.CashAccountNumber}'";
            Assert.Equal(msg, result.NonSuccessMessage);
        }

        [Fact]
        public async void Process_CashAccountUpdateCommand_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();

            OperationResult<bool> result =
                await CashAccountEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CashAccountDeleteCommand_ShouldSucceed()
        {
            CashAccountWriteModel model
                = new()
                {
                    CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                };

            OperationResult<bool> result =
                await CashAccountDeleteCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void CreateCashAccount_CashAccountApplicationService_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();

            OperationResult<bool> result = await _appService.CreateCashAccount(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async void UpdateCashAccount_CashAccountApplicationService_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();

            OperationResult<bool> result = await _appService.UpdateCashAccount(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async void UpdateCashAccount_CashAccountApplicationService_CannotChangeAcctType_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfoWithAcctTypeUpdate();

            OperationResult<bool> result = await _appService.UpdateCashAccount(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async void UpdateCashAccount_CashAccountApplicationService_ExistingAcctName_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Primary Checking";

            OperationResult<bool> result = await _appService.UpdateCashAccount(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async void UpdateCashAccount_CashAccountApplicationService_ChangeAcctTypeNoTrans_ShouldSucceed()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountType = 3;

            OperationResult<bool> result = await _appService.UpdateCashAccount(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async void UpdateCashAccount_CashAccountApplicationService_CannotChangeAcctTypeWithTransactions_ShouldFail()
        {
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfoWithAcctTypeUpdate();

            OperationResult<bool> result = await _appService.UpdateCashAccount(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async void DeleteCashAccount_CashAccountApplicationService_ShouldSucceed()
        {
            CashAccountWriteModel model
                = new()
                {
                    CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                };

            OperationResult<bool> result = await _appService.DeleteCashAccount(model);

            Assert.True(result.Success);
        }

        [Fact]
        public async void DeleteCashAccount_CashAccountApplicationService_AcctHasTransactions_ShouldFail()
        {
            CashAccountWriteModel model
                = new()
                {
                    CashAccountId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589"),
                    UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
                };

            OperationResult<bool> result = await _appService.DeleteCashAccount(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForDebtIssueProceedsCommand_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();

            OperationResult<bool> result =
                await CreateCashDepositForDebtIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForDebtIssueProceedsCommand_DuplicateLoanProceedsDeposit_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDuplicateLoanProceedsDeposit();

            OperationResult<bool> result =
                await CreateCashDepositForDebtIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_InvalidInvestorId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.AgentId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_InvalidEvent_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.EventId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_ExistingButWrongAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.AgentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.TransactionDate = new DateTime(2022, 5, 26);

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.TransactionAmount = 5700M;

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForStockIssueProceedsCommand_ProceedsAlreadyRcvd_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfo_ProceedsAlreadyRcvd();

            OperationResult<bool> result =
                await CreateCashDepositForStockIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_InvalidAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.AgentId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_InvalidEventId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.EventId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_ExistingButWrongInvestor_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.AgentId = new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d");

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.TransactionDate = new DateTime(2022, 1, 2);

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.TransactionAmount = 99.99M;

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForDividendPaymentCommand_AlreadyPaid_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymtPaid();
            model.TransactionAmount = 99.99M;

            OperationResult<bool> result =
                await CreateCashDisbursementForDividendPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForLoanPaymentCommand_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();

            OperationResult<bool> result =
                await CreateCashDisbursementForLoanPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForLoanPaymentCommand_Duplicate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();

            OperationResult<bool> result =
                await CreateCashDisbursementForLoanPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForLoanPaymentCommand_ProceedsNotReceived_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtNoProeedsDeposited();

            OperationResult<bool> result =
                await CreateCashDisbursementForLoanPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CashAccountTransferCreateCommand_ShouldSucceed()
        {
            CashAccountTransferWriteModel model = CashAccountTestData.GetCreateCashAccountTransferInfo();
            model.CashTransferAmount = 10000M;

            OperationResult<bool> result =
                await CashAccountTransferCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        // [Fact]
        // public async void Process_CashAccountTransferCreateCommand_InsufficientFunds_ShouldFail()
        // {
        //     CashAccountTransferWriteModel model = CashAccountTestData.GetCreateCashAccountTransferInfo();
        //     model.CashTransferAmount = 35625.01M;

        //     OperationResult<bool> result =
        //         await CashAccountTransferCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

        //     Assert.False(result.Success);
        // }

    }
}
