#pragma warning disable CS8618

using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.WriteModels.Financing
{
    public class CashAccountWriteModel : IWriteModel
    {
        public Guid CashAccountId { get; set; }
        public int CashAccountType { get; set; }
        public string? BankName { get; set; }
        public string? CashAccountName { get; set; }
        public string? CashAccountNumber { get; set; }
        public string? RoutingTransitNumber { get; set; }
        public DateTime DateOpened { get; set; }
        public Guid UserId { get; set; }
    }

    public class CashTransactionWriteModel : IWriteModel
    {
        public int CashTransactionId { get; set; }
        public Guid CashAccountId { get; set; }
        public int TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public Guid AgentId { get; set; }
        public Guid EventId { get; set; }
        public string? CheckNumber { get; set; }
        public string? RemittanceAdvice { get; set; }
        public Guid UserId { get; set; }
    }

    public class CashAccountTransferWriteModel : IWriteModel
    {
        public Guid CashTransferId { get; set; }
        public Guid SourceCashAccountId { get; set; }
        public Guid DestinationCashAccountId { get; set; }
        public DateTime CashTransferDate { get; set; }
        public decimal CashTransferAmount { get; set; }
        public Guid UserId { get; set; }
    }
}
