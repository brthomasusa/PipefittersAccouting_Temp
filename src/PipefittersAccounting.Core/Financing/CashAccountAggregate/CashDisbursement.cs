using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashDisbursement : CashAccountTransaction
    {

        public CashDisbursement
        (
            CashTransactionTypeEnum disbursementType,
            ExternalAgent payee,
            EconomicEvent goodsOrServicePurchased,

            decimal receiptAmount,
            DateTime receiptDate,
            string checkNumber,
            string remittanceAdvice,
            Guid userId,
            ICashTransactionValidationService validationService
        )
            : base(CashTransactionAmount.Create(receiptAmount),
                   CashTransactionDate.Create(receiptDate),
                   CheckNumber.Create(checkNumber),
                   RemittanceAdvice.Create(remittanceAdvice),
                   EntityGuidID.Create(userId),
                   validationService)
        {
            DisbursementType = disbursementType;
            Payee = payee;
            GoodsOrServicePurchased = goodsOrServicePurchased;

            CheckValidity();
        }

        public CashTransactionTypeEnum DisbursementType { get; init; }

        public ExternalAgent Payee { get; init; }

        public EconomicEvent GoodsOrServicePurchased { get; init; }

        protected override void CheckValidity()
        {
            RejectDeposits();

            ValidationResult result = new();

            result = ValidationService.IsValidCashDisbursement(DisbursementType, GoodsOrServicePurchased, Payee, TransactionAmount).Result;
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Messages[0]);
            }
        }

        private void RejectDeposits()
        {
            switch (DisbursementType)
            {
                // These are not valid for cash disbursements
                case CashTransactionTypeEnum.CashReceiptAdjustment:         // Adjustment to previous deposit
                case CashTransactionTypeEnum.CashReceiptDebtIssueProceeds:  // Deposit of debt issue proceeds
                case CashTransactionTypeEnum.CashReceiptSales:              // Deposit of cash received from customer for product sales
                case CashTransactionTypeEnum.CashReceiptStockIssueProceeds: // Deposit of stock issue proceeds
                case CashTransactionTypeEnum.CashReceiptCashTransferIn:     // Cash transfer into account
                    throw new ArgumentException($"Only cash disbursements (no receipts) allowed: {DisbursementType}");
            }
        }

        private void CheckValidity(EventTypeEnum typeOfGoodsOrServiceProvided)
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