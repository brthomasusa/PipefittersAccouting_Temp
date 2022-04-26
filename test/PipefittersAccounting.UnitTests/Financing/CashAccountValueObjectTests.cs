#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class CashAccountValueObjectTests
    {
        [Fact]
        public void Create_CashReceipt_TooManyDecimalsInAmount_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000.321M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashReceipt_AmountNotGreaterThanZero_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 0M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashReceipt_DefaultDate_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime();
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement);
            string checkNumber = "1234";
            string remitAdvice = "Remit advice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            CashDisbursement disbursement
                = new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            Assert.IsType<CashDisbursement>(disbursement);
        }
    }
}