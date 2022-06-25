using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IEmployeePayrollService
    {
        Task<OperationResult<bool>> Process(Guid cashAccountId, Guid userId);
    }
}