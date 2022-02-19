using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateQueryService : IEmployeeAggregateQueryService
    {
        public Task<OperationResult<EmployeeDetail>> Query(GetEmployee queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployees queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<EmployeeManager>>> Query(GetEmployeeManagers queryParameters)
        {
            throw new NotImplementedException();
        }
    }
}