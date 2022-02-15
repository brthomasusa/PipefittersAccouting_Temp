using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeCreateCommand
    {
        public static async Task<OperationResult> Execute
        (
            CreateEmployeeInfo model,
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            if (await repo.Exists(model.Id))
            {
                InvalidOperationException ex = new InvalidOperationException($"Can not create this employee, they already exists!");
                return OperationResult.ExceptionResult(ex);
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

                return OperationResult.SuccessResult();
            }
            catch (Exception ex)
            {
                return OperationResult.ExceptionResult(ex);
            }
        }
    }
}