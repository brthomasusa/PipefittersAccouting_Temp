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
    public class CashDepositTests
    {
        [Fact]
        public void Create_CashDeposit_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            var exception = Record.Exception(() =>
                                new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId)
                            );

            Assert.Null(exception);
        }

        [Fact]
        public void Create_CashDeposit_Reject_CashDisbursementAdjustment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementAdjustment;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_Reject_CashDisbursementCashTransferOut_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementCashTransferOut;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_Reject_CashDisbursementDividentPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementDividentPayment;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_Reject_CashDisbursementLoanPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_Reject_CashDisbursementPurchaseReceipt_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementPurchaseReceipt;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_Reject_CashDisbursementTimeCardPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementTimeCardPayment;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_InvalidGoodsSvc_LoanPaymentCashDisbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_DividentPaymentCashDisbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_TimeCardPaymentCashDisbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.TimeCardPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_InventoryReceiptCashDisbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidTransTypeGoodsSvc_LoanIssueProceeds_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptSales;
            decimal receiptAmount = 1000M;
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
        public void Create_CashDeposit_InvalidTransTypeGoodsSvc_StockIssueProceeds_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptSales;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.StockSubscription);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_LoanPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_DividendPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_TimeCardPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementTimeCardPayment;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.TimeCardPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidGoodsSvc_InventoryReceipt_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementPurchaseReceipt;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidPayor_InventoryReceipt_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidPayor_TimeCardPayment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.TimeCardPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_InvalidPayor_FinancierSales_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.Sales);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDeposit_ValidPayor_CustomerSales_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptSales;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            ExternalAgent payor = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Customer);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.Sales);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            var exception = Record.Exception(() =>
                                new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId)
                            );

            Assert.Null(exception);
        }

    }
}
// Valid Goods/Svc
// Sales = 1,
// LoanAgreement = 2,
// StockSubscription = 3,

// Invalid Goods/Svc
// LoanPayment = 4,
// DividentPayment = 5,
// TimeCardPayment = 6,
// InventoryReceipt = 7

// Valid transaction types
// CashReceiptSales = 1,
// CashReceiptDebtIssueProceeds = 2,
// CashReceiptStockIssueProceeds = 3,