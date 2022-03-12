using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class FinancierDependencyCheckResult : IReadModel
    {
        public Guid FinancierId { get; set; }
        public int LoanAgreements { get; set; }
        public int StockSubscriptions { get; set; }
    }

    public class FinancierDetail : IReadModel
    {
        public Guid FinancierId { get; set; }
        public string? FinancierName { get; set; }
        public string? Telephone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateCode { get; set; }
        public string? Zipcode { get; set; }
        public string? FullAddress { get; set; }
        public string? ContactFirstName { get; set; }
        public string? ContactLastName { get; set; }
        public string? ContactMiddleInitial { get; set; }
        public string? ContactFullName { get; set; }
        public string? ContactTelephone { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class FinancierListItems : IReadModel
    {
        public Guid FinancierId { get; set; }
        public string? FinancierName { get; set; }
        public string? Telephone { get; set; }
        public string? FullAddress { get; set; }
        public string? ContactFullName { get; set; }
        public string? ContactTelephone { get; set; }
        public bool IsActive { get; set; }
    }

    public class FinancierLookup : IReadModel
    {
        public Guid FinancierId { get; set; }
        public string? FinancierName { get; set; }
    }
}