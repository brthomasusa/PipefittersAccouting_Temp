#pragma warning disable CS8618

using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.WriteModels.HumanResources
{
    public class CreateEmployeeInfo : IWriteModel
    {
        public Guid Id { get; set; }
        public Guid SupervisorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string SSN { get; set; }
        public string Telephone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zipcode { get; set; }
        public string MaritalStatus { get; set; }
        public int Exemptions { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSupervisor { get; set; }
    }

    public class EditEmployeeInfo : IWriteModel
    {
        public Guid Id { get; set; }
        public Guid SupervisorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string SSN { get; set; }
        public string Telephone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zipcode { get; set; }
        public string MaritalStatus { get; set; }
        public int Exemptions { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSupervisor { get; set; }
    }

    public class DeleteEmployeeInfo : IWriteModel
    {
        public Guid Id { get; set; }
    }
}