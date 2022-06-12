using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.FinancierAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface IFinancierAggregateRepository : IAggregateRootRepository<Financier>
    {
        Task<OperationResult<Guid>> CheckForDuplicateFinancierName(string name);
    }
}