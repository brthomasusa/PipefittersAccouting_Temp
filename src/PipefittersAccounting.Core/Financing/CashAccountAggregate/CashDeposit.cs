using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public class CashDeposit : CashAccountTransaction
    {
        public CashDeposit
        (
            CashTransactionTypeEnum depositType,
            ExternalAgent payor,
            EconomicEvent goodsOrServiceSold,
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
            DepositType = depositType;
            Payor = payor;
            GoodsOrServiceSold = goodsOrServiceSold;

            CheckValidity();
        }

        public CashTransactionTypeEnum DepositType { get; init; }

        public ExternalAgent Payor { get; init; }

        public EconomicEvent GoodsOrServiceSold { get; init; }

        protected override void CheckValidity()
        {
            RejectInvalidDeposits();
            RejectInvalidGoodsOrServiceProvided();
        }

        private void RejectInvalidDeposits()
        {
            switch (DepositType)
            {
                // As this is a cash deposit, all cash disbursements are invalid
                case CashTransactionTypeEnum.CashDisbursementAdjustment:        // Adjustment to previous disbursement                
                case CashTransactionTypeEnum.CashDisbursementCashTransferOut:   // Cash transfer out of account
                case CashTransactionTypeEnum.CashDisbursementDividentPayment:   // Dividend Payment
                case CashTransactionTypeEnum.CashDisbursementLoanPayment:       // Loan Payment
                case CashTransactionTypeEnum.CashDisbursementPurchaseReceipt:   // Purchase Order Payment
                case CashTransactionTypeEnum.CashDisbursementTimeCardPayment:   // Employee Paycheck payment
                    throw new ArgumentException($"Only cash deposits (no disbursements) allowed: {DepositType}");
            }
        }

        private void RejectInvalidGoodsOrServiceProvided()
        {
            switch (GoodsOrServiceSold.EventType)
            {
                // Only these would cause an inflow (receipt) of cash
                case EventTypeEnum.LoanAgreementCashReceipt:
                case EventTypeEnum.SalesReceipt:
                case EventTypeEnum.StockSubscriptionCashReceipt:
                    break;

                default:
                    // All other events would cause an outflow (disbursement) of cash
                    string msg = $"Invalid goods or services listed as reason for cash receipt: {GoodsOrServiceSold.EventType}";
                    throw new ArgumentException(msg);
            }

            if (Payor.AgentType == AgentTypeEnum.Financier)
            {
                /*
                    Cash receipt from a financier can only occur because of the previous 
                    signing of a loan agreement or stock subscription by the financier.
                    A financier and a sales receipt are invalid. A customer and a sales receipt would be valid.
                */
                if (GoodsOrServiceSold.EventType != EventTypeEnum.LoanAgreementCashReceipt &&
                    GoodsOrServiceSold.EventType != EventTypeEnum.StockSubscriptionCashReceipt)
                {
                    string msg = $"Invalid goods/services: '{GoodsOrServiceSold.EventType}' listed as reason for cash receipt from financier!";
                    throw new ArgumentException(msg);
                }
            }
        }
    }
}