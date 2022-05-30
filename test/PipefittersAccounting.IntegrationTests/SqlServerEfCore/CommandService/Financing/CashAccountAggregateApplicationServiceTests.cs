#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using Xunit;

using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    public class CashAccountAggregateApplicationServiceTests : TestBaseEfCore
    {
        private readonly ICashAccountApplicationService _appService;
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private readonly AppUnitOfWork _unitOfWork;

        public CashAccountAggregateApplicationServiceTests()
        {
            ICashAccountQueryService queryService = new CashAccountQueryService(_dapperCtx);
            _repository = new CashAccountAggregateRepository(_dbContext);
            _validationService = new CashAccountAggregateValidationService(queryService);
            _unitOfWork = new AppUnitOfWork(_dbContext);
            _appService = new CashAccountApplicationService(_validationService, _repository, _unitOfWork);
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
                await CashAccountUpdateCommand.Process(model, _repository, _validationService, _unitOfWork);

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
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();

            OperationResult<bool> result =
                await CreateCashDepositForDebtIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDepositForDebtIssueProceedsCommand_DuplicateLoanProceedsDeposit_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoDuplicateLoanProceedsDeposit();

            OperationResult<bool> result =
                await CreateCashDepositForDebtIssueProceedsCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForLoanPaymentCommand_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();

            OperationResult<bool> result =
                await CreateCashDisbursementForLoanPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForLoanPaymentCommand_Duplicate_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();

            OperationResult<bool> result =
                await CreateCashDisbursementForLoanPaymentCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async void Process_CreateCashDisbursementForLoanPaymentCommand_ProceedsNotReceived_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtNoProeedsDeposited();

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

        [Fact]
        public async void Process_CashAccountTransferCreateCommand_InsufficientFunds_ShouldFail()
        {
            CashAccountTransferWriteModel model = CashAccountTestData.GetCreateCashAccountTransferInfo();
            model.CashTransferAmount = 35625.01M;

            OperationResult<bool> result =
                await CashAccountTransferCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

    }
}
