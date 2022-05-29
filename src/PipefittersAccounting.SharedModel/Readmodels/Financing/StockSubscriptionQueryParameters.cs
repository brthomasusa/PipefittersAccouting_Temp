namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class GetStockSubscriptionParameters
    {
        public Guid StockId { get; set; }
    }

    public class GetStockSubscriptionListItemParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}