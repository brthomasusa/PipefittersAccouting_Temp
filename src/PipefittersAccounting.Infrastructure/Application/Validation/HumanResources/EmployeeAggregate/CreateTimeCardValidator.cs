using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate
{
    public class CreateTimeCardValidator : ValidatorBase<TimeCardWriteModel, IQueryServicesRegistry>
    {
        public CreateTimeCardValidator
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

            SharedQueryService sharedQueryService
                = QueryServicesRegistry.GetService<SharedQueryService>("SharedQueryService");

            VerifyEmployeeOnTimeCardIsValidRule verifyEmployeeRule = new(sharedQueryService);
            VerifyEmployeeSupervisorLinkRule verifySupervisorRule = new(employeeQueryService);
            VerifyPayPeriodEndedDateIsMostRecentRule verifyPayPeriodEndingDate = new(employeeQueryService);

            verifyEmployeeRule.SetNext(verifySupervisorRule);
            verifySupervisorRule.SetNext(verifyPayPeriodEndingDate);

            return await verifyEmployeeRule.Validate(WriteModel);
        }
    }
}