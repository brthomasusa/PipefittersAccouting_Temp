using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Core.Interfaces.HumanResources
{
    public interface IEmployeeAggregateRepository : IAggregateRootRepository<Employee>
    {
        Task<OperationResult<bool>> DeleteTimeCardAsync(Guid timeCardId);
    }
}
