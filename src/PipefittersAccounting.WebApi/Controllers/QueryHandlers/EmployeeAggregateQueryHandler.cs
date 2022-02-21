using PipefittersAccounting.WebApi.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.WebApi.Controllers.QueryHandlers
{
    public class EmployeeAggregateQueryHandler : IEmployeeAggregateQueryHandler
    {
        private readonly IEmployeeAggregateQueryService _queryService;

        public EmployeeAggregateQueryHandler(IEmployeeAggregateQueryService qrySvc) => _queryService = qrySvc;

        public async Task<OperationResult<IReadModel>> Handle<TQueryParam>(TQueryParam queryParam) =>
            throw new NotImplementedException();

    }
}