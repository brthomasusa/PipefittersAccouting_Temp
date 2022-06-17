#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class VerifyTimeCardPaymentRule : BusinessRule<TimeCardWriteModel>
    {
        private readonly IEmployeeAggregateQueryService _qrySvc;

        public VerifyTimeCardPaymentRule(IEmployeeAggregateQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(TimeCardWriteModel timeCard)
        {
            ValidationResult validationResult = new();

            GetTimeCardParameter queryParameters =
                new() { TimeCardId = timeCard.TimeCardId };

            OperationResult<TimeCardPaymentVerification> result =
                await _qrySvc.GetTimeCardPaymentVerification(queryParameters);

            if (result.Success)
            {
                if (result.Result.AmountPaid == 0)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(timeCard);
                    }
                }
                else
                {
                    string msg = $"This time card cannot be edited or deleted? The employee was paid ${result.Result.AmountPaid} on {result.Result.DatePaid}";
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