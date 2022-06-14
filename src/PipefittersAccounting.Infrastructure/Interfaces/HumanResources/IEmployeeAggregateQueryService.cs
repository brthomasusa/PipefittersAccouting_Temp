
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateQueryService
    {
        Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployeeParameter queryParameters);
        Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployeesParameters queryParameters);
        Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagersParameters queryParameters);
        Task<OperationResult<Guid>> VerifyEmployeeSSNIsUnique(UniqueEmployeSSNParameter queryParameters);
        Task<OperationResult<Guid>> VerifyEmployeeNameIsUnique(UniqueEmployeeNameParameters queryParameters);
        Task<OperationResult<int>> GetCountOfEmployeeTimeCards(GetEmployeeParameter queryParameters);
    }
}
