

namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class StockSubscriptionDetails
    {
        public Guid StockId { get; set; }
        public Guid FinancierId { get; set; }
        public string? InvestorName { get; set; }
        public string? StreetAddress { get; set; }
        public string? CityStateZipcode { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTelephone { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssured { get; set; }
        public decimal PricePerShare { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class StockSubscriptionListItem
    {
        public Guid StockId { get; set; }
        public string? InvestorName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTelephone { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssued { get; set; }
        public decimal PricePerShare { get; set; }
    }

    public class VerificationOfCashDepositStockIssueProceeds
    {
        public Guid FinancierId { get; set; }
        public Guid StockId { get; set; }
        public string? InvestorName { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssued { get; set; }
        public decimal PricePerShare { get; set; }
        public int CashTransactionId { get; set; }
        public DateTime DateReceived { get; set; }
        public decimal AmountReceived { get; set; }
    }

    public class VerifyCashDisbursementForDividendPayment
    {
        public Guid StockId { get; set; }
        public Guid DividendId { get; set; }
        public DateTime DividendDeclarationDate { get; set; }
        public decimal DividendDeclared { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class DividendDeclarationDetails
    {
        public Guid StockId { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssured { get; set; }
        public decimal PricePerShare { get; set; }
        public Guid DividendId { get; set; }
        public DateTime DividendDeclarationDate { get; set; }
        public decimal DividendPerShare { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class DividendDeclarationListItem
    {
        public Guid StockId { get; set; }
        public Guid DividendId { get; set; }
        public DateTime DividendDeclarationDate { get; set; }
        public decimal DividendPerShare { get; set; }
        public decimal DividendPayable { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
    }
}

