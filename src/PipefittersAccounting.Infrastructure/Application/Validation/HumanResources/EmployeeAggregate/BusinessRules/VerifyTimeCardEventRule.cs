#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class VerifyTimeCardEventRule : BusinessRule<TimeCardWriteModel>
    {
        private readonly IEmployeeAggregateQueryService _qrySvc;

        public VerifyTimeCardEventRule(IEmployeeAggregateQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(TimeCardWriteModel timeCard)
        {
            ValidationResult validationResult = new();

            GetTimeCardParameter queryParameters =
                new() { TimeCardId = timeCard.TimeCardId };

            OperationResult<TimeCardVerification> result =
                await _qrySvc.VerifyTimeCardEvent(queryParameters);

            if (result.Success)
            {
                if (timeCard.EmployeeId == result.Result.EmployeeId)
                {
                    if (timeCard.SupervisorId == result.Result.SupervisorId)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next?.Validate(timeCard);
                        }
                    }
                    else
                    {
                        string msg = "The supervisor on the time card is invalid.";
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    string msg = $"Check the employee id provided, it does not match the employee id on saved time card '{result.Result.EmployeeId}'.";
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