using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate
{
    public class EditTimeCardValidator : ValidatorBase<TimeCardWriteModel, IQueryServicesRegistry>
    {
        public EditTimeCardValidator
        (
            TimeCardWriteModel writeModel,
            IQueryServicesRegistry queryServicesRegistry

        ) : base(writeModel, queryServicesRegistry)
        {

        }

        public override async Task<ValidationResult> Validate()
        {
            EmployeeAggregateQueryService employeeQueryService
                = QueryServicesRegistry.GetService<EmployeeAggregateQueryService>("EmployeeAggregateQueryService");

            VerifyTimeCardEventRule verifyTimeCardInfoRule = new(employeeQueryService);
            VerifyTimeCardPaymentRule verifyNotPaidRule = new(employeeQueryService);

            verifyTimeCardInfoRule.SetNext(verifyNotPaidRule);

            return await verifyTimeCardInfoRule.Validate(WriteModel);
        }
    }
}