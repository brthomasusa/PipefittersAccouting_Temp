using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.FinancierAggregate
{
    public class FinancierApplicationService : IFinancierApplicationService
    {
        private readonly IFinancierAggregateRepository _financierRepo;
        private readonly IUnitOfWork _unitOfWork;

        public FinancierApplicationService(IFinancierAggregateRepository repo, IUnitOfWork unitOfWork)
        {
            _financierRepo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateFinancierInfo(FinancierWriteModel writeModel)
            => await FinancierCreateCommand.Execute(writeModel, _financierRepo, _unitOfWork);

        public async Task<OperationResult<bool>> EditFinancierInfo(FinancierWriteModel writeModel)
            => await FinancierEditCommand.Execute(writeModel, _financierRepo, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteFinancierInfo(FinancierWriteModel writeModel)
            => await FinancierDeleteCommand.Execute(writeModel, _financierRepo, _unitOfWork);
    }
}