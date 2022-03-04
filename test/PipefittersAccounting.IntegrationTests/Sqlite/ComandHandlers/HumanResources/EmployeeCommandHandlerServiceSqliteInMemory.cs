using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.IntegrationTests.Sqlite.ComandHandlers.HumanResources
{
    public class EmployeeCommandHandlerServiceSqliteInMemory : IEmployeeAggregateCommandService
    {
        private readonly IEmployeeAggregateRepository _employeeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeCommandHandlerServiceSqliteInMemory
        (
            IEmployeeAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
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
    }
}