using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces.HumanResources
{
    public interface IEmployeeAggregateValidationService : IValidationService
    {
        Task<ValidationResult> IsValidCreateEmployeeInfo(EmployeeWriteModel writeModel);
        Task<ValidationResult> IsValidEditEmployeeInfo(EmployeeWriteModel writeModel);
        Task<ValidationResult> IsValidDeleteTimeCardInfo(EmployeeWriteModel writeModel);
        Task<ValidationResult> IsValidCreateTimeCardInfo(TimeCardWriteModel writeModel);
        Task<ValidationResult> IsValidEditTimeCardInfo(TimeCardWriteModel writeModel);
        Task<ValidationResult> IsValidDeleteTimeCardInfo(TimeCardWriteModel writeModel);
    }
}