using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.IntegrationTests.Sqlite.ComandHandlers.HumanResources
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
            if (await repo.Exists(model.Id) == false)
            {
                ArgumentException ex = new ArgumentException($"Delete failed, an employee with id: {model.Id} could not be found!");
                return OperationResult<bool>.CreateFailure(ex);
            }

            try
            {
                Employee employee = await repo.GetByIdAsync(model.Id);

                repo.Delete(employee);
                await unitOfWork.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex);
            }
        }
    }
}