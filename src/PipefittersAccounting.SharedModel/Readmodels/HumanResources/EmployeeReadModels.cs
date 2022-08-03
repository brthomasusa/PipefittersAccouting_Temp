using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.Readmodels.HumanResources
{
    public class EmployeeDetail : IReadModel
    {
        public Guid EmployeeId { get; set; }
        public Guid SupervisorId { get; set; }
        public int EmployeeTypeId { get; set; }
        public string? EmployeeTypeName { get; set; }
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
        public string? Group { get; set; }
    }

    public class EmployeeTypes
    {
        public int EmployeeTypeId { get; set; }
        public string? EmployeeTypeName { get; set; }
    }

    public class TimeCardDetail
    {
        public Guid TimeCardId { get; set; }
        public Guid EmployeeId { get; set; }
        public string? EmployeeFullName { get; set; }
        public Guid SupervisorId { get; set; }
        public string? ManagerFullName { get; set; }
        public string? MaritalStatus { get; set; }
        public int Exemptions { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PayPeriodEnded { get; set; }
        public int RegularHours { get; set; }
        public int OvertimeHours { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class TimeCardListItem
    {
        public Guid TimeCardId { get; set; }
        public string? ManagerFullName { get; set; }
        public DateTime PayPeriodEnded { get; set; }
        public int RegularHours { get; set; }
        public int OvertimeHours { get; set; }
    }

    public class TimeCardWithPymtInfo
    {
        public Guid TimeCardId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid SupervisorId { get; set; }
        public string? EmployeeFullName { get; set; }
        public DateTime PayPeriodEnded { get; set; }
        public int RegularHours { get; set; }
        public int OvertimeHours { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DatePaid { get; set; }
        public Guid UserId { get; set; }
    }

    public class TimeCardVerification
    {
        public Guid TimeCardId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid SupervisorId { get; set; }
    }

    public class TimeCardPaymentVerification
    {
        public DateTime PayPeriodEnded { get; set; }
        public int RegularHours { get; set; }
        public int OverTimeHours { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class PayrollRegister
    {
        public Guid TimeCardId { get; set; }
        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime PayPeriodEnded { get; set; }
        public decimal RegularPay { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal GrossPay { get; set; }
        public decimal FICA { get; set; }
        public decimal Medicare { get; set; }
        public decimal FederalWithholding { get; set; }
        public decimal NetPay { get; set; }
    }
}