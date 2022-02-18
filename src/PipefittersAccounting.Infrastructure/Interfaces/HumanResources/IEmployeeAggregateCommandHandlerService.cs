using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateCommandHandlerService
    {
        Task<OperationResult<bool>> CreateEmployeeInfo(CreateEmployeeInfo writeModel);
        Task<OperationResult<bool>> EditEmployeeInfo(EditEmployeeInfo writeModel);
        Task<OperationResult<bool>> DeleteEmployeeInfo(DeleteEmployeeInfo writeModel);
    }
}