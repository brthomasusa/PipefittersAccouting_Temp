using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateQueryService : IEmployeeAggregateQueryService
    {
        public Task<OperationResult<EmployeeDetail>> GetEmployeeDetails(GetEmployee queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PagedList<EmployeeListItem>>> GetEmployeeListItems(GetEmployees queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<EmployeeManager>>> GetEmployeeManagers(GetEmployeeManagers queryParameters)
        {
            throw new NotImplementedException();
        }
    }
}