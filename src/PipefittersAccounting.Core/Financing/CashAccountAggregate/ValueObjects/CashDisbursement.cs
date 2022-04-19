using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects
{
    public class CashDisbursement : CashAccountTransaction
    {

        private CashDisbursement
        (
            CashTransactionTypeEnum disbursementType,
            Guid payee,
            Guid goodsOrServiceReceived,

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
            CashTransactionType = disbursementType;
            Payee = EntityGuidID.Create(payee);
            GoodsOrServiceReceived = EntityGuidID.Create(goodsOrServiceReceived);

            CheckValidity(disbursementType);
        }

        public CashTransactionTypeEnum CashTransactionType { get; init; }
        public EntityGuidID Payee { get; init; }
        public EntityGuidID GoodsOrServiceReceived { get; init; }

        public static CashDisbursement Create
        (
            CashTransactionTypeEnum disbursementType,
            decimal receiptAmount,
            DateTime receiptDate,
            EntityGuidID payee,
            EntityGuidID goodsOrServiceReceived,
            string checkNumber,
            string remittanceAdvice,
            Guid userId
        )
        {
            return new CashDisbursement(disbursementType, payee, goodsOrServiceReceived, receiptAmount, receiptDate, checkNumber, remittanceAdvice, userId);
        }

        private void CheckValidity(CashTransactionTypeEnum disbursementType)
        {
            switch (disbursementType)
            {
                // These are not valid for cash disbursements
                case CashTransactionTypeEnum.CashReceiptAdjustment:
                case CashTransactionTypeEnum.CashReceiptDebtIssueProceeds:
                case CashTransactionTypeEnum.CashReceiptSales:
                case CashTransactionTypeEnum.CashReceiptStockIssueProceeds:
                    throw new ArgumentException($"Only cash disbursements (no receipts) allowed: {disbursementType}");
            }

            ValidationResult result = new();

            result = _validationService.IsValidCashDisbursement(disbursementType, GoodsOrServiceReceived, Payee, TransactionAmount).Result;
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Messages[0]);
            }
        }

        private static void CheckValidity(EventTypeEnum typeOfGoodsOrServiceProvided)
        {
            switch (typeOfGoodsOrServiceProvided)
            {
                case EventTypeEnum.LoanAgreementCashReceipt:
                case EventTypeEnum.SalesCashReceipt:
                case EventTypeEnum.StockSubscriptionCashReceipt:
                    throw new ArgumentException($"Invalid goods or services listed as reason for cash disbursement: {typeOfGoodsOrServiceProvided}");
            }
        }
    }
}