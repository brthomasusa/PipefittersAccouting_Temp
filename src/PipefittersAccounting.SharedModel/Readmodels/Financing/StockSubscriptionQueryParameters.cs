namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class GetStockSubscriptionParameter
    {
        public Guid StockId { get; set; }
    }

    public class GetInvestorIdentificationParameter
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

    public class GetDividendDeclarationsParameters
    {
        public Guid StockId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetDividendDeclarationParameter
    {
        public Guid DividendId { get; set; }
    }
}