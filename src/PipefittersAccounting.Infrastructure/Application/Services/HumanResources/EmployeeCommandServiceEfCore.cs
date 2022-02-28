
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;


namespace PipefittersAccounting.Infrastructure.Application.Services.HumanResources
{
    public class EmployeeCommandServiceEfCore : IEmployeeAggregateCommandService
    {
        private readonly IEmployeeAggregateRepository _employeeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeCommandServiceEfCore(IEmployeeAggregateRepository repo, IUnitOfWork unitOfWork)
        {
            _employeeRepo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateEmployeeInfo(CreateEmployeeInfo writeModel) =>
            await EmployeeCreateCommand.Execute(writeModel, _employeeRepo, _unitOfWork);

        public async Task<OperationResult<bool>> EditEmployeeInfo(EditEmployeeInfo writeModel) =>
            await EmployeeEditCommand.Execute(writeModel, _employeeRepo, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteEmployeeInfo(DeleteEmployeeInfo writeModel) =>
            await EmployeeDeleteCommand.Execute(writeModel, _employeeRepo, _unitOfWork);

        public Task<OperationResult<bool>> CheckForDuplicateEmployeeName(CheckForDuplicateEmployeeName name)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> CheckForDuplicateSSN(CheckForDuplicateSSN ssn)
             => throw new NotImplementedException();
    }
}