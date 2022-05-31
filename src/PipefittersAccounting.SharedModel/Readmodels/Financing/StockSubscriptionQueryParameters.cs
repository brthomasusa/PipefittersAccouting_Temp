namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class GetStockSubscriptionParameters
    {
        public Guid StockId { get; set; }
    }

    public class GetInvestorIdentificationParameters
    {
        public Guid FinancierId { get; set; }
    }

    public class GetStockSubscriptionListItemParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class UniqueStockSubscriptionParameters
    {
        public Guid FinancierId { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssued { get; set; }
        public decimal PricePerShare { get; set; }
    }

    public class VerifyCashDepositOfStockIssueProceedsParameters
    {
        public Guid FinancierId { get; set; }
        public Guid StockId { get; set; }
    }
}