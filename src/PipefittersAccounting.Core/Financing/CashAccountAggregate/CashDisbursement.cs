using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
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

            decimal disbursementAmount,
            DateTime disbursementDate,
            string checkNumber,
            string remittanceAdvice,
            Guid userId
        )
            : base(CashTransactionAmount.Create(disbursementAmount),
                   CashTransactionDate.Create(disbursementDate),
                   CheckNumber.Create(checkNumber),
                   RemittanceAdvice.Create(remittanceAdvice),
                   EntityGuidID.Create(userId))
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
            RejectInvalidTransactionTypes();                            // Invalid transaction types
            RejectInvalidTransactionTypeAndGoodsOrServiceProvided();    // Invalid transaction type / GoodsOrServicePurchased combinations
            RejectInvalidPayees();                                      // Invalid payees 
            RejectInvalidGoodsOrServiceReceived();                      // Invalid payee/GoodsOrServicePurchased combinations                     
        }

        private void RejectInvalidTransactionTypes()
        {
            switch (DisbursementType)
            {
                // These are receipts, adjustments, or transfers and are not valid for cash disbursements

                case CashTransactionTypeEnum.CashReceiptAdjustment:             // Adjustment to previous deposit
                case CashTransactionTypeEnum.CashDisbursementAdjustment:        // Adjustment to previous disbursement will be handled by CashAdjustment transaction
                case CashTransactionTypeEnum.CashDisbursementCashTransferOut:   // Transfer out to another cash account will be handled by CashTransfer transaction
                case CashTransactionTypeEnum.CashReceiptCashTransferIn:         // Transfer in from another cash account will be handled by CashTransfer transaction
                case CashTransactionTypeEnum.CashReceiptDebtIssueProceeds:      // Deposit of debt issue proceeds
                case CashTransactionTypeEnum.CashReceiptSales:                  // Deposit of cash received from customer for product sales
                case CashTransactionTypeEnum.CashReceiptStockIssueProceeds:     // Deposit of stock issue proceeds
                    throw new ArgumentException($"Only cash disbursements (no receipts, adjustments, or transfers) allowed: {DisbursementType}");
            }
        }

        private void RejectInvalidGoodsOrServiceReceived()
        {
            switch (GoodsOrServicePurchased.EventType)
            {
                // Only these would cause an outflow (disbursement) of cash
                case EventTypeEnum.DividentPayment:
                case EventTypeEnum.InventoryReceipt:
                case EventTypeEnum.LoanPayment:
                case EventTypeEnum.TimeCardPayment:
                    break;

                default:
                    // All other events would cause an inflow (receipt) of cash or are adjustments and transfers
                    throw new ArgumentException($"Invalid goods or services listed as reason for cash disbursement: {GoodsOrServicePurchased.EventType}");
            }
        }

        private void RejectInvalidPayees()
        {
            /* 
                For each payee, there are a limited number of goods/services that they can provide
                and, thus, legitimately be paid for (receive a cash disbursement). For instance, a
                financier does not provide inventory or labor and, therefore, should not receive a
                disbursement for providing inventory and labor.
            */
            if (Payee.AgentType == AgentTypeEnum.Financier)
            {
                // Only allow disbursements for dividend and loan payments
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.DividentPayment &&
                    GoodsOrServicePurchased.EventType != EventTypeEnum.LoanPayment)
                {
                    string msg = $"A financier can not receive a cash disbursement for {GoodsOrServicePurchased.EventType}";
                    throw new ArgumentException(msg);
                }
            }

            if (Payee.AgentType == AgentTypeEnum.Customer)
            {
                // Don't allow any disbursements; refunds are handled as CashAdjustment
                string msg = $"A customer can not receive a cash disbursement, they can receive a refund (cash receipt adjustment).";
                throw new ArgumentException(msg);
            }

            if (Payee.AgentType == AgentTypeEnum.Vendor)
            {
                // Only allow payments for inventory purchases
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.InventoryReceipt)
                {
                    string msg = $"A vendor can not receive a cash disbursement for {GoodsOrServicePurchased.EventType}";
                    throw new ArgumentException(msg);
                }
            }

            if (Payee.AgentType == AgentTypeEnum.Employee)
            {
                // Only allow payments for payroll
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.TimeCardPayment)
                {
                    string msg = $"An employee can not receive a cash disbursement for {GoodsOrServicePurchased.EventType}";
                    throw new ArgumentException(msg);
                }
            }
        }

        private void RejectInvalidTransactionTypeAndGoodsOrServiceProvided()
        {
            // Check for invalid transaction type / GoodsOrServiceProvided combinations

            if (DisbursementType == CashTransactionTypeEnum.CashDisbursementDividentPayment)
            {
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.DividentPayment)
                {
                    string msg = $"Invalid goods or services '{GoodsOrServicePurchased.EventType}' listed as reason for cash disbursement to an investor!";
                    throw new ArgumentException(msg);
                }
            }

            if (DisbursementType == CashTransactionTypeEnum.CashDisbursementLoanPayment)
            {
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.LoanPayment)
                {
                    string msg = $"Invalid goods or services '{GoodsOrServicePurchased.EventType}' listed as reason for cash disbursement to a creditor!";
                    throw new ArgumentException(msg);
                }
            }

            if (DisbursementType == CashTransactionTypeEnum.CashDisbursementPurchaseReceipt)
            {
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.InventoryReceipt)
                {
                    string msg = $"Invalid goods or services '{GoodsOrServicePurchased.EventType}' listed as reason for cash disbursement to a vendor!";
                    throw new ArgumentException(msg);
                }
            }

            if (DisbursementType == CashTransactionTypeEnum.CashDisbursementTimeCardPayment)
            {
                if (GoodsOrServicePurchased.EventType != EventTypeEnum.TimeCardPayment)
                {
                    string msg = $"Invalid goods or services '{GoodsOrServicePurchased.EventType}' listed as reason for cash disbursement to an employee!";
                    throw new ArgumentException(msg);
                }
            }
        }
    }
}
