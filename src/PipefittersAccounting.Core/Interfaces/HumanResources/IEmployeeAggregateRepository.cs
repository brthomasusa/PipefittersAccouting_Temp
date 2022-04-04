using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Core.Interfaces.HumanResources
{
    public interface IEmployeeAggregateRepository : IRepository<Employee>
    {
        Task<OperationResult<Guid>> CheckForDuplicateEmployeeName(string lname, string fname, string mi);
        Task<OperationResult<Guid>> CheckForDuplicateSSN(string ssn);
    }
}