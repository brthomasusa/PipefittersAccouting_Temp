
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateQueryService
    {
        Task<OperationResult<EmployeeDetail>> Query(GetEmployee queryParameters);
        Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployees queryParameters);
        Task<OperationResult<List<EmployeeManager>>> Query(GetEmployeeManagers queryParameters);
    }
}