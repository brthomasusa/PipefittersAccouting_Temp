using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class EmployeeAggregateQueryHandler : IEmployeeAggregateQueryHandler
    {
        public Task<TypedOperationResult<IReadModel>> Handle<TQueryParam>(TQueryParam queryParam)
        {
            throw new NotImplementedException();
        }
    }
}