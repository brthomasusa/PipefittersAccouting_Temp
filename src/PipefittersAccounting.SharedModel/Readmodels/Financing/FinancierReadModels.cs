using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class EmployeeDependencyCheckResult : IReadModel
    {
        public Guid FinancierId { get; set; }
        public int LoanAgreements { get; set; }
        public int StockSubscriptions { get; set; }
    }

    public class FinancierDetail : IReadModel
    {
        public Guid Id { get; set; }
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
    }

    public class FinancierListItems : IReadModel
    {
        public Guid FinancierId { get; set; }
    }

    // new EditFinancierInfo()
    // {
    //     Id = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a"),
    //             FinancierName = "Bertha Mae Jones Innovative Financing",
    //             Telephone = "886-587-0001",
    //             AddressLine1 = "12333 Menard Heights Blvd",
    //             AddressLine2 = "Suite 1001",
    //             City = "Palo Alto",
    //             StateCode = "CA",
    //             Zipcode = "94901",
    //             ContactFirstName = "Betty",
    //             ContactLastName = "Sinosky",
    //             ContactMiddleInitial = "L",
    //             ContactTelephone = "886-587-0001",
    //             IsActive = true,
    //             UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
    //         };    
}