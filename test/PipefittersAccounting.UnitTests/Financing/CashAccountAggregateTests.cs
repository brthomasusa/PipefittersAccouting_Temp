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

        /*        CashDeposit Tests        */

        [Fact]
        public void Create_CashDeposit_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreementCashReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            var exception = Record.Exception(() =>
                                new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId)
                            );

            Assert.Null(exception);
        }

        [Fact]
        public void Create_CashDeposit_InvalidTransactionType_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementAdjustment;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreementCashReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodOrService_Sales_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.SalesReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodOrService_Disbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPaymentCashDisbursement);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }


        /*        CashDisbursement Tests        */





        /*        CashAccount Tests        */

        [Fact]
        public void Create_CashAccount_ShouldSucceed()
        {
            var mock = new Mock<ICashTransactionValidationService>();
            ICashTransactionValidationService validationService = mock.Object;

            var exception = Record.Exception(() =>
                                CashAccountTestData.GetCashAccount(validationService)
                            );

            Assert.Null(exception);
        }

        [Fact]
        public async Task AddDeposit_CashAccount_ShouldSucceed()
        {
            CashDeposit cashDeposit = CashAccountTestData.GetCashDepositForLoanProceeds();

            var mock = new Mock<ICashTransactionValidationService>();
            mock.Setup(x => x.IsValidCashDeposit(It.IsAny<CashDeposit>())).ReturnsAsync(new ValidationResult() { IsValid = true });
            ICashTransactionValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccount(validationService);

            await cashAccount.AddDeposit(cashDeposit);

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

            var mock = new Mock<ICashTransactionValidationService>();
            mock.Setup(x => x.IsValidCashDeposit(It.IsAny<CashDeposit>())).ReturnsAsync(result);
            ICashTransactionValidationService validationService = mock.Object;

            CashAccount cashAccount = CashAccountTestData.GetCashAccount(validationService);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await cashAccount.AddDeposit(cashDeposit));
            Assert.Equal("Thrown by Moq", ex.Message);
        }
    }
}