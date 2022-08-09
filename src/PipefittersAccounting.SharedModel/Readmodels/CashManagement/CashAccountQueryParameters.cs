
namespace PipefittersAccounting.SharedModel.Readmodels.CashManagement
{
    public class ReceiptLoanProceedsValidationParams
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
    }

    public class CreditorLoanAgreementValidationParameters
    {
        public Guid FinancierId { get; set; }
        public Guid LoanId { get; set; }
    }

    public class GetCashAccount
    {
        public Guid CashAccountId { get; set; }
    }

    public class GetCashAccounts
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetCashAccountWithAccountName
    {
        public string? AccountName { get; set; }
    }

    public class GetCashAccountWithAccountNumber
    {
        public string? AccountNumber { get; set; }
    }

    public class GetLoanInstallmentInfoParameters
    {
        public Guid LoanInstallmentId { get; set; }
    }

    public class GetCashAccountTransactionDetailParameters
    {
        public int CashTransactionId { get; set; }
    }

    public class GetCashAccountTransactionListItemsParameters
    {
        public Guid CashAccountId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetInvestorIdForStockSubscriptionParameter
    {
        public Guid StockId { get; set; }
    }

    public class GetTimeCardPaymentInfoParameter
    {
        public DateTime PayPeriodBegin { get; set; }
        public DateTime PayPeriodEnd { get; set; }
    }
}