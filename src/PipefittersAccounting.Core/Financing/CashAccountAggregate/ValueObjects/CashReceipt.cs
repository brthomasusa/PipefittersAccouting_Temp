using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects
{
    public class CashReceipt : CashAccountTransaction
    {
        private CashReceipt
        (
            CashTransactionTypeEnum receiptType,
            Guid payor,
            Guid goodsOrServiceProvided,

            decimal receiptAmount,
            DateTime receiptDate,
            string checkNumber,
            string remittanceAdvice,
            Guid userId
        )
            : base(CashTransactionAmount.Create(receiptAmount),
                   CashTransactionDate.Create(receiptDate),
                   CheckNumber.Create(checkNumber),
                   RemittanceAdvice.Create(remittanceAdvice),
                   EntityGuidID.Create(userId))
        {
            CashTransactionType = receiptType;
            Payor = EntityGuidID.Create(payor);
            GoodsOrServiceProvided = EntityGuidID.Create(goodsOrServiceProvided);
        }

        public CashTransactionTypeEnum CashTransactionType { get; init; }

        public Guid Payor { get; init; }

        public Guid GoodsOrServiceProvided { get; init; }

        public static CashReceipt Create
        (
            CashTransactionTypeEnum receiptType,
            decimal receiptAmount,
            DateTime receiptDate,
            Guid payor,
            Guid goodsOrServiceProvided,
            string checkNumber,
            string remittanceAdvice,
            Guid userId
        )
        {
            CheckValidity(receiptType);

            return new CashReceipt(receiptType, payor, goodsOrServiceProvided, receiptAmount, receiptDate, checkNumber, remittanceAdvice, userId);
        }

        private static void CheckValidity(CashTransactionTypeEnum receiptType)
        {
            switch (receiptType)
            {
                case CashTransactionTypeEnum.CashDisbursementAdjustment:
                case CashTransactionTypeEnum.CashDisbursementCashTransferIn:
                case CashTransactionTypeEnum.CashDisbursementCashTransferOut:
                case CashTransactionTypeEnum.CashDisbursementDividentPayment:
                case CashTransactionTypeEnum.CashDisbursementLoanPayment:
                case CashTransactionTypeEnum.CashDisbursementPurchaseReceipt:
                case CashTransactionTypeEnum.CashDisbursementTimeCardPayment:
                    throw new ArgumentException(nameof(receiptType), $"Only cash receipts (no disbursements) allowed: {receiptType}");
            }
        }

        private static void CheckValidity(EventTypeEnum typeOfGoodsOrServiceProvided)
        {
            switch (typeOfGoodsOrServiceProvided)
            {
                case EventTypeEnum.DividentPaymentCashDisbursement:
                case EventTypeEnum.InventoryReceiptCashDisbursement:
                case EventTypeEnum.LoanPaymentCashDisbursement:
                case EventTypeEnum.TimeCardPaymentCashDisbursement:
                    throw new ArgumentException($"Invalid goods or services listed as reason for cash receipt: {typeOfGoodsOrServiceProvided}");
            }
        }
    }
}