#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class CashAccountAggregateTests
    {
        public CashAccountAggregateTests() { }

        [Fact]
        public void Create_CashAccount_ShouldSucceed()
        {
            var mock = new Mock<ICashAccountAggregateValidationService>();
            ICashAccountAggregateValidationService validationService = mock.Object;

            var exception = Record.Exception(() =>
                                CashAccountTestData.GetCashAccount(validationService)
                            );

            Assert.Null(exception);
        }

        [Fact]
        public async Task AddDeposit_CashAccount_ShouldSucceed()
        {
            CashDeposit cashDeposit = CashAccountTestData.GetCashDepositForLoanProceeds();

            var mock = new Mock<ICashAccountAggregateValidationService>();
            mock.Setup(x => x.IsValidCashDeposit(It.IsAny<CashDeposit>())).ReturnsAsync(new ValidationResult() { IsValid = true });
            ICashAccountAggregateValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccount(validationService);

            await cashAccount.DepositCash(cashDeposit);

            mock.Verify(x => x.IsValidCashDeposit(It.IsAny<CashDeposit>()), Times.Once);
            int transactions = cashAccount.CashTransactions.Count;
            Assert.Equal(1, transactions);
        }

        [Fact]
        public async Task AddDeposit_CashAccount_ShouldThrowException()
        {
            CashDeposit cashDeposit = CashAccountTestData.GetCashDepositForLoanProceeds();
            ValidationResult result = new() { IsValid = false };
            result.Messages.Add("Thrown by Moq");

            var mock = new Mock<ICashAccountAggregateValidationService>();
            mock.Setup(x => x.IsValidCashDeposit(It.IsAny<CashDeposit>())).ReturnsAsync(result);
            ICashAccountAggregateValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccount(validationService);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await cashAccount.DepositCash(cashDeposit));
            Assert.Equal("Thrown by Moq", ex.Message);
        }
    }
}