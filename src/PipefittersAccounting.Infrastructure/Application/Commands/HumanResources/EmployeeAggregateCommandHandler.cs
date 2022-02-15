using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeAggregateCommandHandler : ICommandHandler
    {
        private readonly IEmployeeAggregateRepository _employeeRepo;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeAggregateCommandHandler(IEmployeeAggregateRepository repo, IUnitOfWork unitOfWork)
        {
            _employeeRepo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(IWriteModel writeModel) =>
            writeModel switch
            {
                CreateEmployeeInfo createModel => await EmployeeCreateCommand.Execute(createModel, _employeeRepo, _unitOfWork),
                EditEmployeeInfo updateModel => await EmployeeEditCommand.Execute(updateModel, _employeeRepo, _unitOfWork),
                DeleteEmployeeInfo deleteModel => await EmployeeDeleteCommand.Execute(deleteModel, _employeeRepo, _unitOfWork),
                _ => throw new ArgumentOutOfRangeException("Unknown employee write command.", nameof(writeModel))
            };
    }
}