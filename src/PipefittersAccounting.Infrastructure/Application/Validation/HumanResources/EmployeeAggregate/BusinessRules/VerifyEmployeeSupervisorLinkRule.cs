#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class VerifyEmployeeSupervisorLinkRule : BusinessRule<TimeCardWriteModel>
    {
        private readonly IEmployeeAggregateQueryService _qrySvc;

        public VerifyEmployeeSupervisorLinkRule(IEmployeeAggregateQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(TimeCardWriteModel timeCard)
        {
            ValidationResult validationResult = new();

            GetEmployeeParameter queryParameters =
                new() { EmployeeID = timeCard.EmployeeId };

            OperationResult<Guid> result =
                await _qrySvc.VerifyEmployeeSupervisorLink(queryParameters);

            if (result.Success)
            {
                if (timeCard.SupervisorId == result.Result)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(timeCard);
                    }
                }
                else
                {
                    string msg = $"The supervisor on the time card does not match the supervisor from the employee table.";
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