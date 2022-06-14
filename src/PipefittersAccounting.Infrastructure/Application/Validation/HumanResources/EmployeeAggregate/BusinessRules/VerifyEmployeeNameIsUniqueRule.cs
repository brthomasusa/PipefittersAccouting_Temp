#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class VerifyEmployeeNameIsUniqueRule : BusinessRule<EmployeeWriteModel>
    {
        private readonly IEmployeeAggregateQueryService _qrySvc;

        public VerifyEmployeeNameIsUniqueRule(IEmployeeAggregateQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(EmployeeWriteModel employee)
        {
            ValidationResult validationResult = new();

            UniqueEmployeeNameParameters queryParameters =
                new() { LastName = employee.LastName, FirstName = employee.FirstName, MiddleInitial = employee.MiddleInitial };

            OperationResult<Guid> result =
                await _qrySvc.VerifyEmployeeNameIsUnique(queryParameters);

            if (result.Success)
            {
                // An empty guid indicates that the name is unique
                if (result.Result == Guid.Empty)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(employee);
                    }
                }
                else
                {
                    string msg = @"Create employee info failed! An employee with 
                                   the same last name, first name, and middle 
                                   intial is alread in the database.";

                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}