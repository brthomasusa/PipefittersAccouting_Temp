using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeDeleteCommand : WriteCommandHandler<EmployeeWriteModel,
                                                           IEmployeeAggregateRepository,
                                                           IEmployeeAggregateValidationService,
                                                           Employee>
    {
        public EmployeeDeleteCommand
        (
            EmployeeWriteModel model,
            IEmployeeAggregateRepository repo,
            IEmployeeAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(model, repo, validationService, unitOfWork)
        {

        }

        protected override async Task<ValidationResult> Validate()
        {
            return await ValidationService.IsValidEditEmployeeInfo(WriteModel);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            try
            {
                OperationResult<Employee> getResult = await Repository.GetByIdAsync(WriteModel.EmployeeId);

                if (getResult.Success)
                {
                    Employee employee = getResult.Result;

                    OperationResult<bool> deleteResult = Repository.Delete(employee);

                    if (deleteResult.Success)
                    {
                        await UnitOfWork.Commit();
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(deleteResult.NonSuccessMessage);
                    }
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(getResult.NonSuccessMessage);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}