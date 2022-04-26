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
            RejectInvalidTransactionTypes();
            RejectInvalidGoodsOrServiceProvided();
            RejectInvalidTransactionTypeAndGoodsOrServiceReceived();
            RejectInvalidPayors();
        }

        private void RejectInvalidTransactionTypes()
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
                case EventTypeEnum.Sales:
                case EventTypeEnum.LoanAgreement:
                case EventTypeEnum.StockSubscription:
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
                if (GoodsOrServiceSold.EventType != EventTypeEnum.LoanAgreement &&
                    GoodsOrServiceSold.EventType != EventTypeEnum.StockSubscription)
                {
                    string msg = $"Invalid goods/services: '{GoodsOrServiceSold.EventType}' listed as reason for cash receipt from financier!";
                    throw new ArgumentException(msg);
                }
            }
        }

        private void RejectInvalidTransactionTypeAndGoodsOrServiceReceived()
        {
            if (DepositType == CashTransactionTypeEnum.CashReceiptSales)
            {
                if (GoodsOrServiceSold.EventType != EventTypeEnum.Sales)
                {
                    string msg = $"Invalid goods or services '{GoodsOrServiceSold.EventType}' listed as reason for cash receipt from a customer!";
                    throw new ArgumentException(msg);
                }
            }

            if (DepositType == CashTransactionTypeEnum.CashReceiptDebtIssueProceeds)
            {
                if (GoodsOrServiceSold.EventType != EventTypeEnum.LoanAgreement)
                {
                    string msg = $"Invalid services '{GoodsOrServiceSold.EventType}' listed as reason for cash receipt from a creditor!";
                    throw new ArgumentException(msg);
                }
            }

            if (DepositType == CashTransactionTypeEnum.CashReceiptStockIssueProceeds)
            {
                if (GoodsOrServiceSold.EventType != EventTypeEnum.StockSubscription)
                {
                    string msg = $"Invalid services '{GoodsOrServiceSold.EventType}' listed as reason for cash receipt from a creditor!";
                    throw new ArgumentException(msg);
                }
            }
        }

        private void RejectInvalidPayors()
        {
            /* 
                For each payor, there are a limited number of goods/services provided to them by
                Pipefitters. For instance, Pipefitters does not sell product to a Financier,
                therefore, should not receive a payment for sales, only loan and stock proceeds.                
            */

            if (Payor.AgentType == AgentTypeEnum.Financier)
            {
                // Only allow receipts for debt issue and stock issue.
                if (GoodsOrServiceSold.EventType != EventTypeEnum.LoanAgreement &&
                    GoodsOrServiceSold.EventType != EventTypeEnum.StockSubscription)
                {
                    string msg = "Only receipts for loan agreement and stock subscription proceeds are valid reasons to receive cash from a financier";
                    throw new ArgumentException(msg);
                }
            }

            if (Payor.AgentType == AgentTypeEnum.Customer)
            {
                // Only allow receipts from product sales.
                if (GoodsOrServiceSold.EventType != EventTypeEnum.Sales)
                {
                    string msg = "Receipt of cash from a customer should only be for product sales.";
                    throw new ArgumentException(msg);
                }
            }

            if (Payor.AgentType == AgentTypeEnum.Vendor)
            {
                // Don't allow any receipts; refunds are handled as CashAdjustment
                string msg = "Should not receive payment from a vendor; refunds for previous purchases are handled as cash disbursement adjustments";
                throw new ArgumentException(msg);
            }

            if (Payor.AgentType == AgentTypeEnum.Employee)
            {
                // Don't allow any receipts; refunds are handled as CashAdjustment
                string msg = "Should not receive payment from an employee, cash from an employee handled as cash disbursement adjustments";
                throw new ArgumentException(msg);
            }
        }
    }
}
