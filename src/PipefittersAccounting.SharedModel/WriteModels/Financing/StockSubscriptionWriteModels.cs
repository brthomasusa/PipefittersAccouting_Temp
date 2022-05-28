#pragma warning disable CS8618

using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.WriteModels.Financing
{
    public class CreateStockSubscriptionInfo : IWriteModel
    {
        public Guid StockId { get; set; }
        public Guid FinancierId { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssued { get; set; }
        public decimal PricePerShare { get; set; }
        public Guid UserId { get; set; }
    }

    public class EditStockSubscriptionInfo : IWriteModel
    {
        public Guid StockId { get; set; }
        public DateTime StockIssueDate { get; set; }
        public int SharesIssued { get; set; }
        public decimal PricePerShare { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeleteStockSubscriptionInfo : IWriteModel
    {
        public Guid StockId { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateDividendDeclarationInfo : IWriteModel
    {
        public Guid DividendId { get; set; }
        public Guid StockId { get; set; }
        public DateTime DividendDeclarationDate { get; set; }
        public decimal DividendPerShare { get; set; }
        public Guid UserId { get; set; }
    }

    public class EditDividendDeclarationInfo : IWriteModel
    {
        public Guid DividendId { get; set; }
        public DateTime DividendDeclarationDate { get; set; }
        public decimal DividendPerShare { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeleteDividendDeclarationInfo : IWriteModel
    {
        public Guid DividendId { get; set; }
        public Guid UserId { get; set; }
    }
}



