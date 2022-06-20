namespace PipefittersAccounting.SharedModel.Readmodels.HumanResources
{
    public class GetEmployeeParameter
    {
        public Guid EmployeeID { get; set; }
    }

    public class GetTimeCardParameter
    {
        public Guid TimeCardId { get; set; }
    }

    public class GetEmployeesParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetTimeCardsWithPagingInfoParameter
    {
        public Guid EmployeeId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class UniqueEmployeeNameParameters
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleInitial { get; set; }
    }

    public class UniqueEmployeSSNParameter
    {
        public string? SSN { get; set; }
    }

    public class GetEmployeeManagersParameters
    {

    }

    public class GetPayrollRegisterParameter
    {
        public DateTime PayPeriodEnded { get; set; }
    }
}