using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeDeleteCommand
    {
        public static async Task<OperationResult> Execute
        (
            DeleteEmployeeInfo model,
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            if (await repo.Exists(model.Id) == false)
            {
                ArgumentException ex = new ArgumentException($"Delete failed, an employee with id: {model.Id} could not be found!");
                return OperationResult.ExceptionResult(ex);
            }

            try
            {
                Employee employee = await repo.GetByIdAsync(model.Id);

                repo.Delete(employee);
                await unitOfWork.Commit();

                return OperationResult.SuccessResult();
            }
            catch (Exception ex)
            {
                return OperationResult.ExceptionResult(ex);
            }
        }
    }
}