#pragma warning disable CS8604

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeCreateCommand : WriteCommandHandler<EmployeeWriteModel,
                                                            IEmployeeAggregateRepository,
                                                            IEmployeeAggregateValidationService,
                                                            Employee>
    {
        public EmployeeCreateCommand
        (
            EmployeeWriteModel model,
            IEmployeeAggregateRepository repo,
            IEmployeeAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(model, repo, validationService, unitOfWork)
        {

        }

        protected override async Task<ValidationResult> Validate()
            => await ValidationService.IsValidCreateEmployeeInfo(WriteModel);

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            try
            {
                Employee employee = new Employee
                (
                    EntityGuidID.Create(WriteModel.EmployeeId),
                    (EmployeeTypeEnum)Enum.ToObject(typeof(EmployeeTypeEnum), WriteModel.EmployeeType),
                    EntityGuidID.Create(WriteModel.SupervisorId),
                    PersonName.Create(WriteModel.LastName, WriteModel.FirstName, WriteModel.MiddleInitial),
                    SocialSecurityNumber.Create(WriteModel.SSN),
                    EmailAddress.Create(WriteModel.EmailAddress),
                    PhoneNumber.Create(WriteModel.Telephone),
                    Address.Create(WriteModel.AddressLine1, WriteModel.AddressLine2, WriteModel.City, WriteModel.StateCode, WriteModel.Zipcode),
                    MaritalStatus.Create(WriteModel.MaritalStatus),
                    TaxExemption.Create(WriteModel.Exemptions),
                    PayRate.Create(WriteModel.PayRate),
                    StartDate.Create(WriteModel.StartDate),
                    WriteModel.IsActive,
                    WriteModel.IsSupervisor
                );

                OperationResult<bool> addResult = await Repository.AddAsync(employee);

                if (addResult.Success)
                {
                    await UnitOfWork.Commit();
                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(addResult.NonSuccessMessage);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}