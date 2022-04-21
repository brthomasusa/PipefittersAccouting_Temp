#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.Financing
{
    public class CashAccountValueObjectTests
    {
        private Mock<ICashTransactionValidationService> _validationSvcMock;
        private ValidationResult _result;

        public CashAccountValueObjectTests()
        {
            _validationSvcMock = new Mock<ICashTransactionValidationService>();
            _result = new();
        }

        // [Fact]
        // public void Create_CashReceipt_ShouldSucceed()
        // {
        //     CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
        //     decimal receiptAmount = 1000M;
        //     DateTime receiptDate = new DateTime(2022, 4, 19);
        //     Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
        //     Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
        //     string checkNumber = "1234";
        //     string remitAdvice = "Remit adice";
        //     Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

        //     _result.IsValid = true;

        //     _validationSvcMock.Setup(p => p.IsValidCashDeposit(transType, EntityGuidID.Create(payor), EntityGuidID.Create(goodsOrSvc), CashTransactionAmount.Create(receiptAmount)))
        //                                    .ReturnsAsync(_result);
        //     ICashTransactionValidationService validationSvc = _validationSvcMock.Object;

        //     CashDeposit receipt = new(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId, _validationSvcMock.Object);

        //     Assert.IsType<CashDeposit>(receipt);
        // }

        // [Fact]
        // public void Create_CashReceipt_InvalidTransactionType_ShouldFail()
        // {
        //     CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementAdjustment;
        //     decimal receiptAmount = 1000M;
        //     DateTime receiptDate = new DateTime(2022, 4, 19);
        //     Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
        //     Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
        //     string checkNumber = "1234";
        //     string remitAdvice = "Remit adice";
        //     Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

        //     _result.IsValid = true;

        //     _validationSvcMock.Setup(p => p.IsValidCashDeposit(transType, EntityGuidID.Create(payor), EntityGuidID.Create(goodsOrSvc), CashTransactionAmount.Create(receiptAmount)))
        //                                    .ReturnsAsync(_result);
        //     ICashTransactionValidationService validationSvc = _validationSvcMock.Object;

        //     Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId, validationSvc);

        //     var caughtException = Assert.Throws<ArgumentException>(action);
        //     Assert.Equal($"Only cash receipts (no disbursements) allowed: {transType}", caughtException.ParamName);
        // }

        // [Fact]
        // public void Create_CashReceipt_TooManyDecimalsInAmount_ShouldFail()
        // {
        //     CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
        //     decimal receiptAmount = 1000.123M;
        //     DateTime receiptDate = new DateTime(2022, 4, 19);
        //     Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
        //     Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
        //     string checkNumber = "1234";
        //     string remitAdvice = "Remit adice";
        //     Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

        //     _result.IsValid = true;

        //     _validationSvcMock.Setup(p => p.IsValidCashDeposit(transType, EntityGuidID.Create(payor), EntityGuidID.Create(goodsOrSvc), CashTransactionAmount.Create(receiptAmount)))
        //                                    .ReturnsAsync(_result);
        //     ICashTransactionValidationService validationSvc = _validationSvcMock.Object; ;

        //     Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId, validationSvc);

        //     var caughtException = Assert.Throws<ArgumentException>(action);
        // }

        // [Fact]
        // public void Create_CashReceipt_AmountNotGreaterThanZero_ShouldFail()
        // {
        //     CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
        //     decimal receiptAmount = 0M;
        //     DateTime receiptDate = new DateTime(2022, 4, 19);
        //     Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
        //     Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
        //     string checkNumber = "1234";
        //     string remitAdvice = "Remit adice";
        //     Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

        //     _result.IsValid = true;

        //     _validationSvcMock.Setup(p => p.IsValidCashDeposit(transType, EntityGuidID.Create(payor), EntityGuidID.Create(goodsOrSvc), CashTransactionAmount.Create(receiptAmount)))
        //                                    .ReturnsAsync(_result);
        //     ICashTransactionValidationService validationSvc = _validationSvcMock.Object; ;

        //     Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId, validationSvc);

        //     var caughtException = Assert.Throws<ArgumentException>(action);
        // }

        // [Fact]
        // public void Create_CashReceipt_DefaultDate_ShouldFail()
        // {
        //     CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashReceiptDebtIssueProceeds;
        //     decimal receiptAmount = 1000M;
        //     DateTime receiptDate = new DateTime();
        //     Guid payor = new Guid("12998229-7ede-4834-825a-0c55bde75695");
        //     Guid goodsOrSvc = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
        //     string checkNumber = "1234";
        //     string remitAdvice = "Remit adice";
        //     Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

        //     _result.IsValid = true;

        //     _validationSvcMock.Setup(p => p.IsValidCashDeposit(transType, EntityGuidID.Create(payor), EntityGuidID.Create(goodsOrSvc), CashTransactionAmount.Create(receiptAmount)))
        //                                    .ReturnsAsync(_result);
        //     ICashTransactionValidationService validationSvc = _validationSvcMock.Object; ;

        //     Action action = () => new CashDeposit(transType, payor, goodsOrSvc, receiptAmount, receiptDate, checkNumber, remitAdvice, userId, validationSvc);

        //     var caughtException = Assert.Throws<ArgumentNullException>(action);
        // }

        // [Fact]
        // public void Create_CashDisbursement_ShouldSucceed()
        // {
        //     CashTransactionTypeEnum transType = CashTransactionTypeEnum.CashDisbursementLoanPayment;
        //     decimal disbursementAmount = 1000M;
        //     DateTime disbursementDate = new DateTime(2022, 4, 19);
        //     Guid payee = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");
        //     Guid goodsOrSvc = new Guid("0bd39edb-8da3-40f9-854f-b90e798b82c2");
        //     string checkNumber = "1234";
        //     string remitAdvice = "Remit advice";
        //     Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

        //     _result.IsValid = true;

        //     _validationSvcMock.Setup(p => p.IsValidCashDisbursement(transType, EntityGuidID.Create(payee), EntityGuidID.Create(goodsOrSvc), CashTransactionAmount.Create(disbursementAmount)))
        //                                    .ReturnsAsync(_result);
        //     ICashTransactionValidationService validationSvc = _validationSvcMock.Object; ;

        //     CashDisbursement disbursement
        //         = new CashDisbursement(transType, payee, goodsOrSvc, disbursementAmount, disbursementDate, checkNumber, remitAdvice, userId, validationSvc);

        //     Assert.IsType<CashDisbursement>(disbursement);
        // }
    }
}