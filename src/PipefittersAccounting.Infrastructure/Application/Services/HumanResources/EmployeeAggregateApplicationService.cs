
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;


namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeAggregateApplicationService : IEmployeeAggregateApplicationService
    {
        private readonly IEmployeeAggregateValidationService _validationService;
        private readonly IEmployeeAggregateRepository _employeeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeAggregateApplicationService
        (
            IEmployeeAggregateValidationService validationService,
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            _validationService = validationService;
            _employeeRepo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateEmployeeInfo(EmployeeWriteModel writeModel)
        {
            EmployeeCreateCommand createCommand = new(writeModel, _employeeRepo, _validationService, _unitOfWork);
            return await createCommand.Process();
        }

        public async Task<OperationResult<bool>> EditEmployeeInfo(EmployeeWriteModel writeModel)
        {
            EmployeeEditCommand editCommand = new(writeModel, _employeeRepo, _validationService, _unitOfWork);
            return await editCommand.Process();
        }

        public async Task<OperationResult<bool>> DeleteEmployeeInfo(EmployeeWriteModel writeModel)
        {
            EmployeeDeleteCommand deleteCommand = new(writeModel, _employeeRepo, _validationService, _unitOfWork);
            return await deleteCommand.Process();
        }
    }
}
