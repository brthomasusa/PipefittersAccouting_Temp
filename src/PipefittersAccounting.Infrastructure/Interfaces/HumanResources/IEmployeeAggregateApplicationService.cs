using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateApplicationService
    {
        Task<OperationResult<bool>> CreateEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> EditEmployeeInfo(EditEmployeeInfo writeModel);
        Task<OperationResult<bool>> DeleteEmployeeInfo(DeleteEmployeeInfo writeModel);
    }
}
