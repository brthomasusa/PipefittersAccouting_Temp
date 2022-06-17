using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate
{
    public class CreateEmployeeValidator : ValidatorBase<EmployeeWriteModel, IQueryServicesRegistry>
    {
        public CreateEmployeeValidator
        (
            EmployeeWriteModel writeModel,
            IQueryServicesRegistry queryServicesRegistry

        ) : base(writeModel, queryServicesRegistry)
        {

        }

        public override async Task<ValidationResult> Validate()
        {
            EmployeeAggregateQueryService employeeQueryService
                = QueryServicesRegistry.GetService<EmployeeAggregateQueryService>("EmployeeAggregateQueryService");

            VerifyEmployeeNameIsUniqueRule uniqueNameRule = new(employeeQueryService);

            return await uniqueNameRule.Validate(WriteModel);
        }
    }
}
