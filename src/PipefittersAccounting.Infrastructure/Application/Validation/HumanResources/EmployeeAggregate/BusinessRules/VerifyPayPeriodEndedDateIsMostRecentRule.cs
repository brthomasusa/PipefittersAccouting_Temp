#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class VerifyPayPeriodEndedDateIsMostRecentRule : BusinessRule<TimeCardWriteModel>
    {
        private readonly IEmployeeAggregateQueryService _qrySvc;

        public VerifyPayPeriodEndedDateIsMostRecentRule(IEmployeeAggregateQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(TimeCardWriteModel timeCard)
        {
            ValidationResult validationResult = new();

            GetMostRecentPayPeriodParameter queryParameters = new() { };

            OperationResult<DateTime> result =
                await _qrySvc.GetMostRecentPayPeriodEndedDate(queryParameters);

            if (result.Success)
            {
                DateTime lastPeriodEndedDate = result.Result;

                if (timeCard.PayPeriodEnded > lastPeriodEndedDate)
                {
                    int pymtYear = lastPeriodEndedDate.Year;
                    int pymtMonth = lastPeriodEndedDate.Month;

                    if (pymtMonth == 12)
                    {
                        pymtYear++;
                        pymtMonth = 1;
                    }
                    else
                    {
                        pymtMonth++;
                    }

                    int daysInMonth = DateTime.DaysInMonth(year: pymtYear, month: pymtMonth);
                    DateTime lastDayOfMonth = new(pymtYear, pymtMonth, daysInMonth);

                    if (timeCard.PayPeriodEnded == lastDayOfMonth)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next?.Validate(timeCard);
                        }
                    }
                    else
                    {
                        string msg = $"Invalid pay period ended date '{result.Result}', it should be {lastDayOfMonth}.";
                        validationResult.Messages.Add(msg);
                    }

                }
                else
                {
                    string msg = $"The time cards pay period ended date must be greater than {lastPeriodEndedDate}.";
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