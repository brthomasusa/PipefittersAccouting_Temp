using PipefittersAccounting.UI.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.UI.Interfaces
{
    public interface IEmployeeRepository
    {
        // Alter system state with WriteModels
        Task<OperationResult<EmployeeDetail>> CreateEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> CreateTimeCardInfo(TimeCardWriteModel writeModel);
        Task<OperationResult<bool>> EditTimeCardInfo(TimeCardWriteModel writeModel);
        Task<OperationResult<bool>> DeleteTimeCardInfo(TimeCardWriteModel writeModel);

        // Query system state with ReadModels        
        Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters);
        Task<OperationResult<PagingResponse<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters);
        Task<OperationResult<PagingResponse<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesByLastNameParameters queryParameters);
        Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers();
        Task<OperationResult<List<EmployeeTypes>>> GetEmployeeTypes();
        Task<OperationResult<TimeCardDetail>> GetEmployeeTimeCardDetails(GetTimeCardParameter queryParameters);
        Task<OperationResult<List<TimeCardListItem>>> GetEmployeeTimeCardListItems(GetEmployeeParameter queryParameters);
        Task<OperationResult<List<PayrollRegister>>> GetPayrollRegister(GetPayrollRegisterParameter queryParameters);
        Task<OperationResult<List<TimeCardWithPymtInfo>>> GetTimeCardsForManager(GetTimeCardsForManagerParameter queryParameters);
        Task<OperationResult<List<TimeCardWithPymtInfo>>> GetTimeCardsForPayPeriod(GetTimeCardsForPayPeriodParameter queryParameters);
    }
}