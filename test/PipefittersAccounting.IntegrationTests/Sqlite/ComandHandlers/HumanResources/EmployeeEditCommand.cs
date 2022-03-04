using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.IntegrationTests.Sqlite.ComandHandlers.HumanResources
{
    public class EmployeeEditCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            EditEmployeeInfo model,
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            if (await repo.Exists(model.Id) == false)
            {
                ArgumentException ex = new ArgumentException($"Update failed, an employee with id: {model.Id} could not be found!");
                return OperationResult<bool>.CreateFailure(ex);
            }

            OperationResult<Guid> dupeAddressResult = await repo.CheckForDuplicateEmployeeName(model.LastName, model.FirstName, model.MiddleInitial);
            if (dupeAddressResult.Result != Guid.Empty && dupeAddressResult.Result != model.Id)
            {

                string errMsg = $"An employee name {model.FirstName} {model.MiddleInitial} {model.LastName} is already in the database.";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            OperationResult<Guid> dupeSSN = await repo.CheckForDuplicateSSN(model.SSN);
            if (dupeSSN.Result != Guid.Empty && dupeSSN.Result != model.Id)
            {
                string errMsg = $"An employee with social security number: {model.SSN} is already in the database.";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            try
            {
                Employee employee = await repo.GetByIdAsync(model.Id);

                employee.UpdateSupervisorId(EntityGuidID.Create(model.SupervisorId));
                employee.UpdateEmployeeName(PersonName.Create(model.LastName, model.FirstName, model.MiddleInitial));
                employee.UpdateSSN(SocialSecurityNumber.Create(model.SSN));
                employee.UpdateEmployeePhoneNumber(PhoneNumber.Create(model.Telephone));
                employee.UpdateEmployeeAddress(Address.Create(model.AddressLine1, model.AddressLine2, model.City, model.StateCode, model.Zipcode));
                employee.UpdateMaritalStatus(MaritalStatus.Create(model.MaritalStatus));
                employee.UpdateTaxExemptions(TaxExemption.Create(model.Exemptions));
                employee.UpdateEmployeePayRate(PayRate.Create(model.PayRate));
                employee.UpdateEmploymentDate(StartDate.Create(model.StartDate));
                employee.UpdateEmployeeStatus(model.IsActive);
                employee.UpdateIsSupervisor(model.IsSupervisor);

                repo.Update(employee);
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