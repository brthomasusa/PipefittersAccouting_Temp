using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class TimeCardDeleteCommand : WriteCommandHandler<TimeCardWriteModel,
                                                             IEmployeeAggregateRepository,
                                                             IEmployeeAggregateValidationService,
                                                             Employee>
    {
        public TimeCardDeleteCommand
        (
            TimeCardWriteModel model,
            IEmployeeAggregateRepository repo,
            IEmployeeAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(model, repo, validationService, unitOfWork)
        {

        }

        protected override async Task<ValidationResult> Validate()
        {
            return await ValidationService.IsValidDeleteTimeCardInfo(WriteModel);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            try
            {
                OperationResult<bool> deleteResult = await Repository.DeleteTimeCardAsync(WriteModel.TimeCardId);

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
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}