using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashDeposit : CashAccountTransaction
    {
        public CashDeposit
        (
            CashTransactionTypeEnum receiptType,
            ExternalAgent payor,
            EconomicEvent goodsOrServiceSold,

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
            DepositType = receiptType;
            Payor = payor;
            GoodsOrServiceSold = goodsOrServiceSold;

            CheckValidity();
        }

        public CashTransactionTypeEnum DepositType { get; init; }

        public ExternalAgent Payor { get; init; }

        public EconomicEvent GoodsOrServiceSold { get; init; }

        protected override void CheckValidity()
        {
            RejectDisbursements();
            RejectInvalidGoodsOrServiceProvided();

            ValidationResult result = new();

            result = ValidationService.IsValidCashDeposit(DepositType, GoodsOrServiceSold, Payor, TransactionAmount).Result;
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Messages[0]);
            }
        }

        private void RejectDisbursements()
        {
            switch (DepositType)
            {
                case CashTransactionTypeEnum.CashDisbursementAdjustment:        // Adjustment to previous payment                
                case CashTransactionTypeEnum.CashDisbursementCashTransferOut:   // Cash transfer out of account
                case CashTransactionTypeEnum.CashDisbursementDividentPayment:   // Dividend Payment
                case CashTransactionTypeEnum.CashDisbursementLoanPayment:       // Loan Payment
                case CashTransactionTypeEnum.CashDisbursementPurchaseReceipt:   // Purchase Order Payment
                case CashTransactionTypeEnum.CashDisbursementTimeCardPayment:   // Paycheck to Employee
                    throw new ArgumentException($"Only cash deposits (no disbursements) allowed: {DepositType}");
            }
        }

        private void RejectInvalidGoodsOrServiceProvided()
        {
            switch (GoodsOrServiceSold.EventType)
            {
                case EventTypeEnum.LoanAgreementCashReceipt:
                case EventTypeEnum.SalesCashReceipt:
                case EventTypeEnum.StockSubscriptionCashReceipt:
                    break;
                default:
                    throw new ArgumentException($"Invalid goods or services listed as reason for cash receipt: {GoodsOrServiceSold.EventType}");
            }
        }
    }
}