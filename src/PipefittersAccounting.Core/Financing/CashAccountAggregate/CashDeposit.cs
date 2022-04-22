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
                case EventTypeEnum.LoanAgreementCashReceipt:
                case EventTypeEnum.SalesCashReceipt:
                case EventTypeEnum.StockSubscriptionCashReceipt:
                    break;
                default:
                    throw new ArgumentException($"Invalid goods or services listed as reason for cash receipt: {GoodsOrServiceSold.EventType}");
            }

            if (Payor.AgentType == AgentTypeEnum.Financier)
            {
                // Cash deposits from a financier can only be because of the provision 
                // of a loan agreement or stock subscription to the financier
                if (GoodsOrServiceSold.EventType != EventTypeEnum.LoanAgreementCashReceipt && GoodsOrServiceSold.EventType != EventTypeEnum.StockSubscriptionCashReceipt)
                {
                    throw new ArgumentException($"Invalid goods or services listed as reason for cash receipt from financier: {GoodsOrServiceSold.EventType}");
                }
            }

        }
    }
}