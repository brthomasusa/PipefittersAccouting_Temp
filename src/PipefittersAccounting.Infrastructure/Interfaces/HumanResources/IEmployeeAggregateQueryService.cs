
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateQueryService
    {
        Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters);
        Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters);
        Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesByLastNameParameters queryParameters);
        Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagersParameters queryParameters);
        Task<OperationResult<Guid>> VerifyEmployeeSSNIsUnique(UniqueEmployeSSNParameter queryParameters);
        Task<OperationResult<Guid>> VerifyEmployeeNameIsUnique(UniqueEmployeeNameParameters queryParameters);
        Task<OperationResult<int>> GetCountOfEmployeeTimeCards(GetEmployeeParameter queryParameters);
        Task<OperationResult<TimeCardDetail>> GetEmployeeTimeCardDetails(GetTimeCardParameter queryParameters);
        Task<OperationResult<List<TimeCardListItem>>> GetEmployeeTimeCardListItems(GetEmployeeParameter queryParameters);
        Task<OperationResult<Guid>> VerifyEmployeeSupervisorLink(GetEmployeeParameter queryParameters);
        Task<OperationResult<DateTime>> GetMostRecentPayPeriodEndedDate(GetMostRecentPayPeriodParameter queryParameters);
        Task<OperationResult<TimeCardVerification>> VerifyTimeCardEvent(GetTimeCardParameter queryParameters);
        Task<OperationResult<TimeCardPaymentVerification>> GetTimeCardPaymentVerification(GetTimeCardParameter queryParameters);
        Task<OperationResult<List<PayrollRegister>>> GetPayrollRegister(GetPayrollRegisterParameter queryParameters);
    }
}