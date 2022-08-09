#pragma warning disable CS8604

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeEditCommand : WriteCommandHandler<EmployeeWriteModel,
                                                           IEmployeeAggregateRepository,
                                                           IEmployeeAggregateValidationService,
                                                           Employee>
    {
        public EmployeeEditCommand
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

                    employee.UpdateSupervisorId(EntityGuidID.Create(WriteModel.SupervisorId));
                    employee.UpdateEmployeeName(PersonName.Create(WriteModel.LastName, WriteModel.FirstName, WriteModel.MiddleInitial));
                    employee.UpdateSSN(SocialSecurityNumber.Create(WriteModel.SSN));
                    employee.UpdateEmployeePhoneNumber(PhoneNumber.Create(WriteModel.Telephone));
                    employee.UpdateEmailAddress(EmailAddress.Create(WriteModel.EmailAddress));
                    employee.UpdateEmployeeAddress(Address.Create(WriteModel.AddressLine1, WriteModel.AddressLine2, WriteModel.City, WriteModel.StateCode, WriteModel.Zipcode));
                    employee.UpdateMaritalStatus(MaritalStatus.Create(WriteModel.MaritalStatus));
                    employee.UpdateTaxExemptions(TaxExemption.Create(WriteModel.Exemptions));
                    employee.UpdateEmployeePayRate(PayRate.Create(WriteModel.PayRate));
                    employee.UpdateEmploymentDate(StartDate.Create(WriteModel.StartDate));
                    employee.UpdateEmployeeStatus(WriteModel.IsActive);
                    employee.UpdateIsSupervisor(WriteModel.IsSupervisor);

                    Repository.Update(employee);
                    await UnitOfWork.Commit();

                    return OperationResult<bool>.CreateSuccessResult(true);
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