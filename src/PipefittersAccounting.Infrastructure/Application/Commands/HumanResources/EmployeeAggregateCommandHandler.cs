using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.HumanResources
{
    public class EmployeeAggregateCommandHandler : IEmployeeAggregateCommandHandler
    {
        private readonly IEmployeeAggregateCommandHandlerService _cmdHdlrSvc;

        public EmployeeAggregateCommandHandler(IEmployeeAggregateCommandHandlerService cmdHdlrService)
            => _cmdHdlrSvc = cmdHdlrService;

        public async Task<OperationResult<bool>> Handle(IWriteModel writeModel) =>
            writeModel switch
            {
                CreateEmployeeInfo createModel => await _cmdHdlrSvc.CreateEmployeeInfo(createModel),
                EditEmployeeInfo updateModel => await _cmdHdlrSvc.EditEmployeeInfo(updateModel),
                DeleteEmployeeInfo deleteModel => await _cmdHdlrSvc.DeleteEmployeeInfo(deleteModel),
                _ => OperationResult<bool>.CreateFailure(new ArgumentOutOfRangeException("Unknown employee write command.", nameof(writeModel)))
            };
    }
}
