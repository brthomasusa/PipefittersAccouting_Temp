using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateApplicationService
    {
        Task<OperationResult<bool>> CreateEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel);
        Task<OperationResult<bool>> CreateTimeCardInfo(TimeCardWriteModel writeModel);
        Task<OperationResult<bool>> EditTimeCardInfo(TimeCardWriteModel writeModel);
        Task<OperationResult<bool>> DeleteTimeCardInfo(TimeCardWriteModel writeModel);
    }
}
