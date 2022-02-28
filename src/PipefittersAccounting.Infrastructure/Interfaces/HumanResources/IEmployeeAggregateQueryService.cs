
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateQueryService
    {
        Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployee queryParameters);
        Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployees queryParameters);
        Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagers queryParameters);
    }
}
