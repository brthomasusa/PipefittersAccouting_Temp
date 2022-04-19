#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using Moq;
using System;
using Xunit;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class CashAccountValueObjectTests
    {
        [Fact]
        public void Create_CashReceipt_ShouldSucceed()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            CashReceipt receipt = CashReceipt.Create(transType, receiptAmount, receiptDate, payor, goodsOrSvc, checkNumber, remitAdvice, userId);

            Assert.IsType<CashReceipt>(receipt);
        }

        [Fact]
        public void Create_CashReceipt_InvalidTransactionType_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementAdjustment;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => CashReceipt.Create(transType, receiptAmount, receiptDate, payor, goodsOrSvc, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
            Assert.Equal($"Only cash receipts (no disbursements) allowed: {transType}", caughtException.ParamName);
        }

        [Fact]
        public void Create_CashReceipt_TooManyDecimalsInAmount_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000.123M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => CashReceipt.Create(transType, receiptAmount, receiptDate, payor, goodsOrSvc, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashReceipt_AmountNotGreaterThanZero_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 0M;
            DateTime receiptDate = new DateTime(2022, 4, 19);
            Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => CashReceipt.Create(transType, receiptAmount, receiptDate, payor, goodsOrSvc, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CashReceipt_DefaultDate_ShouldFail()
        {
            CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
            decimal receiptAmount = 1000M;
            DateTime receiptDate = new DateTime();
            Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
            string checkNumber = "1234";
            string remitAdvice = "Remit adice";
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            Action action = () => CashReceipt.Create(transType, receiptAmount, receiptDate, payor, goodsOrSvc, checkNumber, remitAdvice, userId);

            var caughtException = Assert.Throws<ArgumentNullException>(action);
        }
    }
}