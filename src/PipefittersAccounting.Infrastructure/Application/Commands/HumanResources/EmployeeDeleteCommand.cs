using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeDeleteCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            DeleteEmployeeInfo model,
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                OperationResult<bool> result = await repo.Exists(model.Id);

                if (!result.Success)
                {
                    string errMsg = $"Delete failed, an employee with id: {model.Id} could not be found!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Employee> getResult = await repo.GetByIdAsync(model.Id);

                if (getResult.Success)
                {
                    Employee employee = getResult.Result;

                    OperationResult<bool> deleteResult = repo.Delete(employee);

                    if (deleteResult.Success)
                    {
                        await unitOfWork.Commit();
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