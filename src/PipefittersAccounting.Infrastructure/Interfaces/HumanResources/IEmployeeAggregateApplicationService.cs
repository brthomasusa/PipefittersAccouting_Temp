using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateApplicationService
    {
        Task<OperationResult<bool>> CreateEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel);
    }
}
