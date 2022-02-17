using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class EmployeeAggregateQueryHandler : IEmployeeAggregateQueryHandler
    {
        private readonly IEmployeeAggregateQueryService _queryService;

        public EmployeeAggregateQueryHandler(IEmployeeAggregateQueryService qrySvc) => _queryService = qrySvc;

        public Task<OperationResult<IReadModel>> Handle<TQueryParam>(TQueryParam queryParam)
        {
            throw new NotImplementedException();
        }
    }
}