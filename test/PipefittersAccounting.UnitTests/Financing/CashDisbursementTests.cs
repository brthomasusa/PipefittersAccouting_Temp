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
    public class CashDisbursementTests
    {
        [Fact]
        public void Create_CashDisbursement_LoanPymt_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            var exception = Record.Exception(() =>
                                new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId)
                            );

            Assert.Null(exception);
        }

        [Fact]
        public void Create_CashDisbursement_DividendPymt_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementDividentPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            var exception = Record.Exception(() =>
                                new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId)
                            );

            Assert.Null(exception);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashReceiptAdjustment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptAdjustment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashDisbursementAdjustment_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementAdjustment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashDisbursementCashTransferOut_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementCashTransferOut;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashReceiptCashTransferIn_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptCashTransferIn;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashReceiptDebtIssueProceeds_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashReceiptSales_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptSales;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashReceiptStockIssueProceeds_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptStockIssueProceeds;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_SalesReceipt_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.Sales);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_LoanAgreementCashReceipt_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_StockSubscriptionCashReceipt_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.StockSubscription);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_TimeCardPaymentCashDisbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.TimeCardPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_InventoryReceiptCashDisbursement_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashDisbursementToVendor_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Vendor);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_CashDisbursementToCustomer_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Customer);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_PymtForInventory_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementDividentPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_PymtForLoan_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementDividentPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_Reject_PymtForPayroll_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementDividentPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.TimeCardPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        //
        [Fact]
        public void Create_CashDisbursement_LoanPymt_Reject_PymtForInventory_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.InventoryReceipt);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_LoanPymt_Reject_PymtForDividend_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.DividentPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashDisbursement_LoanPymt_Reject_PymtForPayroll_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
            decimal disbursementAmount = 1000M;
            DateTime disbursementDate = new DateTime(2022, 4, 19);
            ExternalAgent payee = new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier);
            EconomicEvent goodsOrSvc = new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.TimeCardPayment);
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

    }
}

// case EventTypeEnum.DividentPaymentCashDisbursement:
// case EventTypeEnum.InventoryReceiptCashDisbursement:
// case EventTypeEnum.LoanPaymentCashDisbursement:
// case EventTypeEnum.TimeCardPaymentCashDisbursement:

