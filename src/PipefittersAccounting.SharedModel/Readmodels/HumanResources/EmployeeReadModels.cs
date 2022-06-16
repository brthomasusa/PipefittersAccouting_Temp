using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.Readmodels.HumanResources
{
    public class SupervisorLookup : IReadModel
    {
        public Guid ManagerId { get; set; }
        public string? ManagerName { get; set; }
    }

    public class EmployeeDetail : IReadModel
    {
        public Guid EmployeeId { get; set; }
        public Guid SupervisorId { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerFirstName { get; set; }
        public string? ManagerMiddleInitial { get; set; }
        public string? ManagerFullName { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleInitial { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? SSN { get; set; }
        public string? Telephone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateCode { get; set; }
        public string? Zipcode { get; set; }
        public string? MaritalStatus { get; set; }
        public int Exemptions { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSupervisor { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class EmployeeListItem
    {
        public Guid EmployeeId { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? Telephone { get; set; }
        public bool IsActive { get; set; }
        public bool IsSupervisor { get; set; }
        public string? ManagerFullName { get; set; }
    }

    public class EmployeeManager
    {
        public Guid ManagerId { get; set; }
        public string? ManagerFullName { get; set; }
    }

    public class ExemptionLookUp
    {
        public int NumberOfExemptions { get; set; }
        public decimal ExemptionAmount { get; set; }
    }

    public class FedWithHolding
    {
        public string? MaritalStatus { get; set; }
        public int FederalTaxBracket { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal TaxRate { get; set; }
        public decimal BracketBaseAmount { get; set; }
    }
}