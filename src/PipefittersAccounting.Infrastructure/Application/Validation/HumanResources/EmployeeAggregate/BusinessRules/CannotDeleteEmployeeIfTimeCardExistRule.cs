#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class CannotDeleteEmployeeIfTimeCardExistRule : BusinessRule<EmployeeWriteModel>
    {
        private readonly IEmployeeAggregateQueryService _qrySvc;

        public CannotDeleteEmployeeIfTimeCardExistRule(IEmployeeAggregateQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(EmployeeWriteModel employee)
        {
            ValidationResult validationResult = new();

            GetEmployeeParameter queryParameters =
                new() { EmployeeID = employee.EmployeeId };

            OperationResult<int> result =
                await _qrySvc.GetCountOfEmployeeTimeCards(queryParameters);

            if (result.Success)
            {
                if (result.Result == 0)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(employee);
                    }
                }
                else
                {
                    string msg = "Delete employee failed! Can not delete an employee with time cards.";
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