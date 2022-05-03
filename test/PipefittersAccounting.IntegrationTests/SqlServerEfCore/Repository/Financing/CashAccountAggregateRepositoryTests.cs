#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.Repository.Financing
{
    [Trait("Integration", "EfCoreRepo")]
    public class CashAccountAggregateRepositoryTests : TestBaseEfCore
    {

        private readonly ICashAccountAggregateRepository _repository;

        public CashAccountAggregateRepositoryTests()
        {
            ICashAccountQueryService qryService = new CashAccountQueryService(_dapperCtx);
            ICashAccountAggregateValidationService validationService = new CashAccountAggregateValidationService(qryService);
            _repository = new CashAccountAggregateRepository(_dbContext);
        }

        [Fact]
        public async Task DoesCashAccountExist_CashAccountAggregateRepository_ShouldReturnTrue()
        {
            Guid acctId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589");
            OperationResult<bool> result = await _repository.DoesCashAccountExist(acctId);

            Assert.True(result.Result);
        }

        [Fact]
        public async Task DoesCashAccountExist_CashAccountAggregateRepository_ShouldReturnFalse()
        {
            Guid acctId = new Guid("123f8a5f-60e7-411a-8e87-dfab0ae62589");
            OperationResult<bool> result = await _repository.DoesCashAccountExist(acctId);

            Assert.False(result.Result);
        }

        [Fact]
        public async Task GetCashAccountByIdAsync_CashAccountAggregateRepository_ShouldSucceed()
        {
            Guid acctId = new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589");
            OperationResult<CashAccount> result = await _repository.GetCashAccountByIdAsync(acctId);

            Assert.True(result.Success);
            Assert.Equal(acctId, result.Result.Id);
        }

        [Fact]
        public async Task AddCashAccountAsync_CashAccountAggregateRepository_ShouldSucceed()
        {
            var mock = new Mock<ICashAccountAggregateValidationService>();
            ICashAccountAggregateValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccount(validationService);
            OperationResult<bool> result = await _repository.AddCashAccountAsync(cashAccount);

            Assert.True(result.Success);

            OperationResult<bool> searchResult = await _repository.DoesCashAccountExist(cashAccount.Id);
            Assert.True(searchResult.Result);
        }

        [Fact]
        public async Task AddCashAccountAsync_CashAccountAggregateRepository_DupeCashAccountId_ShouldFail()
        {
            var mock = new Mock<ICashAccountAggregateValidationService>();
            ICashAccountAggregateValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccountWithDuplicateCashAccountId(validationService);
            OperationResult<bool> result = await _repository.AddCashAccountAsync(cashAccount);

            Assert.False(result.Success);

            string msg = $"A cash account with id '{cashAccount.Id}' is already in the database.";
            Assert.Equal(msg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task AddCashAccountAsync_CashAccountAggregateRepository_HasDupeAcctNumber_ShouldFail()
        {
            var mock = new Mock<ICashAccountAggregateValidationService>();
            ICashAccountAggregateValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccountWithDupeAcctNumber(validationService);
            OperationResult<bool> result = await _repository.AddCashAccountAsync(cashAccount);

            Assert.False(result.Success);

            string msg = $"A cash account with account number '{cashAccount.CashAccountNumber}' is already in the database.";
            Assert.Equal(msg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task AddCashAccountAsync_CashAccountAggregateRepository_HasDupeAcctName_ShouldFail()
        {
            var mock = new Mock<ICashAccountAggregateValidationService>();
            ICashAccountAggregateValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccount(validationService);
            cashAccount.UpdateCashAccountName(CashAccountName.Create("Slush Fund"));
            OperationResult<bool> result = await _repository.AddCashAccountAsync(cashAccount);

            Assert.False(result.Success);

            string msg = $"A cash account with account name '{cashAccount.CashAccountName}' is already in the database.";
            Assert.Equal(msg, result.NonSuccessMessage);
        }


    }
}