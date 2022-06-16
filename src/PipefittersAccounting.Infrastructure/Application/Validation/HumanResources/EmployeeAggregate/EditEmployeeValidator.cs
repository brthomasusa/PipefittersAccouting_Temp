using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate
{
    public class EditEmployeeValidator : ValidatorBase<EmployeeWriteModel, IQueryServicesRegistry>
    {
        public EditEmployeeValidator
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

            SharedQueryService sharedQueryService
                = QueryServicesRegistry.GetService<SharedQueryService>("SharedQueryService");

            VerifyAgentIsEmployeeRule verifyAgentIsEmployee = new(sharedQueryService);
            VerifyEmployeeNameIsUniqueRule uniqueNameRule = new(employeeQueryService);

            verifyAgentIsEmployee.SetNext(uniqueNameRule);

            return await verifyAgentIsEmployee.Validate(WriteModel);
        }
    }
}