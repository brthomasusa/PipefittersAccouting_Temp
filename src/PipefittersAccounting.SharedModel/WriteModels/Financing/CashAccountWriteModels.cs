#pragma warning disable CS8618

using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.WriteModels.Financing
{
    public class CreateCashAccountInfo : IWriteModel
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

    public class EditCashAccountInfo : IWriteModel
    {
        public Guid CashAccountId { get; set; }
        public int CashAccountType { get; set; }
        public string? BankName { get; set; }
        public string? CashAccountName { get; set; }
        public string? RoutingTransitNumber { get; set; }
        public DateTime DateOpened { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeleteCashAccountInfo : IWriteModel
    {
        public Guid CashAccountId { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateCashDepositInfo : IWriteModel
    {
        public int DepositType { get; set; }
        public Guid Payor { get; set; }
        public Guid GoodsOrServices { get; set; }
        public decimal DepositAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public string? CheckNumber { get; set; }
        public string? RemittanceAdvice { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateCashDisbursementInfo : IWriteModel
    {
        public int DisbursementType { get; set; }
        public Guid Payee { get; set; }
        public Guid GoodsOrServices { get; set; }
        public decimal DisbursementAmount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string? CheckNumber { get; set; }
        public string? RemittanceAdvice { get; set; }
        public Guid UserId { get; set; }
    }
}
