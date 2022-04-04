using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeCreateCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            CreateEmployeeInfo model,
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            if (await repo.Exists(model.Id))
            {
                return OperationResult<bool>.CreateFailure($"Can not create this employee, they already exists!");
            }

            OperationResult<Guid> dupeAddressResult = await repo.CheckForDuplicateEmployeeName(model.LastName, model.FirstName, model.MiddleInitial);
            if (dupeAddressResult.Result != Guid.Empty)
            {
                string errMsg = $"An employee name {model.FirstName} {model.MiddleInitial} {model.LastName} is already in the database.";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            OperationResult<Guid> dupeSSN = await repo.CheckForDuplicateSSN(model.SSN);
            if (dupeSSN.Result != Guid.Empty)
            {
                string errMsg = $"An employee with social security number: {model.SSN} is already in the database.";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            try
            {
                Employee employee = new Employee
                (
                    EmployeeAgent.Create(EntityGuidID.Create(model.Id)),
                    EntityGuidID.Create(model.SupervisorId),
                    PersonName.Create(model.LastName, model.FirstName, model.MiddleInitial),
                    SocialSecurityNumber.Create(model.SSN),
                    PhoneNumber.Create(model.Telephone),
                    Address.Create(model.AddressLine1, model.AddressLine2, model.City, model.StateCode, model.Zipcode),
                    MaritalStatus.Create(model.MaritalStatus),
                    TaxExemption.Create(model.Exemptions),
                    PayRate.Create(model.PayRate),
                    StartDate.Create(model.StartDate),
                    model.IsActive,
                    model.IsSupervisor
                );

                await repo.AddAsync(employee);
                await unitOfWork.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}