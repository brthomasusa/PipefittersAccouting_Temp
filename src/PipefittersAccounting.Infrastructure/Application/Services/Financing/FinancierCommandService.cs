using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class FinancierCommandService : IFinancierCommandService
    {
        private readonly IFinancierAggregateRepository _financierRepo;
        private readonly IUnitOfWork _unitOfWork;

        public FinancierCommandService(IFinancierAggregateRepository repo, IUnitOfWork unitOfWork)
        {
            _financierRepo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateFinancierInfo(CreateFinancierInfo writeModel)
            => await FinancierCreateCommand.Execute(writeModel, _financierRepo, _unitOfWork);

        public async Task<OperationResult<bool>> EditFinancierInfo(EditFinancierInfo writeModel)
            => await FinancierEditCommand.Execute(writeModel, _financierRepo, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteFinancierInfo(DeleteFinancierInfo writeModel)
            => await FinancierDeleteCommand.Execute(writeModel, _financierRepo, _unitOfWork);
    }
}