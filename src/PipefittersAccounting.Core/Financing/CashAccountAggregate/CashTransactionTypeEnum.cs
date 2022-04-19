namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public enum CashTransactionTypeEnum : int
    {
        CashReceiptSales = 1,
        CashReceiptDebtIssueProceeds = 2,
        CashReceiptStockIssueProceeds = 3,
        CashDisbursementLoanPayment = 4,
        CashDisbursementDividentPayment = 5,
        CashDisbursementTimeCardPayment = 6,
        CashDisbursementPurchaseReceipt = 7,
        CashReceiptAdjustment = 8,
        CashDisbursementAdjustment = 9,
        CashDisbursementCashTransferOut = 10,
        CashDisbursementCashTransferIn = 11
    }
}
