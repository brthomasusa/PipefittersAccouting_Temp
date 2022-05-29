

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
        public int SharesIssued { get; set; }
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
}

