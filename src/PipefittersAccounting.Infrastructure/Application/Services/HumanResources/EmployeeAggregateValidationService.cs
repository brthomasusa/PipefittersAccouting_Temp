using PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateValidationService : IEmployeeAggregateValidationService
    {
        private IQueryServicesRegistry _servicesRegistry;

        public EmployeeAggregateValidationService
        (
            IEmployeeAggregateQueryService employeeQrySvc,
            ISharedQueryService sharedQueryService,
            IQueryServicesRegistry servicesRegistry
        )
        {
            _servicesRegistry = servicesRegistry;

            _servicesRegistry.RegisterService("EmployeeAggregateQueryService", employeeQrySvc);
            _servicesRegistry.RegisterService("SharedQueryService", sharedQueryService);
        }

        public async Task<ValidationResult> IsValidCreateEmployeeInfo(EmployeeWriteModel writeModel)
        {
            CreateEmployeeValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidEditEmployeeInfo(EmployeeWriteModel writeModel)
        {
            EditEmployeeValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate(); ;
        }

        public async Task<ValidationResult> IsValidDeleteTimeCardInfo(EmployeeWriteModel writeModel)
        {
            DeleteEmployeeValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidCreateTimeCardInfo(TimeCardWriteModel writeModel)
        {
            CreateTimeCardValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidEditTimeCardInfo(TimeCardWriteModel writeModel)
        {
            EditTimeCardValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidDeleteTimeCardInfo(TimeCardWriteModel writeModel)
        {
            DeleteTimeCardValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }
    }
}