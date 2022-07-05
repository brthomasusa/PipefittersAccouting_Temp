using PipefittersAccounting.UI.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.UI.Interfaces
{
    public interface IEmployeeHttpService
    {
        // Alter system state with WriteModels
        Task<OperationResult<bool>> CreateEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> CreateTimeCardInfo(TimeCardWriteModel writeModel);
        Task<OperationResult<bool>> EditTimeCardInfo(TimeCardWriteModel writeModel);
        Task<OperationResult<bool>> DeleteTimeCardInfo(TimeCardWriteModel writeModel);

        // Query system state with ReadModels        
        Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters);
        Task<OperationResult<PagingResponse<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters);
        Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagersParameters queryParameters);
        Task<OperationResult<TimeCardDetail>> GetEmployeeTimeCardDetails(GetTimeCardParameter queryParameters);
        Task<OperationResult<List<TimeCardListItem>>> GetEmployeeTimeCardListItems(GetEmployeeParameter queryParameters);
        Task<OperationResult<List<PayrollRegister>>> GetPayrollRegister(GetPayrollRegisterParameter queryParameters);
    }
}