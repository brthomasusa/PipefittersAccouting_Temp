namespace PipefittersAccounting.Core.Shared
{
    public enum EventTypeEnum : int
    {
        SalesReceipt = 1,
        LoanAgreementCashReceipt = 2,
        StockSubscriptionCashReceipt = 3,
        LoanPaymentCashDisbursement = 4,
        DividentPaymentCashDisbursement = 5,
        TimeCardPaymentCashDisbursement = 6,
        InventoryReceiptCashDisbursement = 7
    }
}
