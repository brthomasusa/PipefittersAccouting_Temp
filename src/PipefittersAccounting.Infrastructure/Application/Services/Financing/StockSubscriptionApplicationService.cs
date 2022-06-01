using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class StockSubscriptionApplicationService : IStockSubscriptionApplicationService
    {
        private readonly IStockSubscriptionValidationService _validationService;
        private readonly IStockSubscriptionAggregateRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public StockSubscriptionApplicationService
        (
            IStockSubscriptionValidationService validationService,
            IStockSubscriptionAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            _validationService = validationService;
            _repository = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateStockSubscription(StockSubscriptionWriteModel writeModel)
            => await StockSubscriptionCreateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> UpdateStockSubscription(StockSubscriptionWriteModel writeModel)
            => await StockSubscriptionEditCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteStockSubscription(StockSubscriptionWriteModel writeModel)
            => await StockSubscriptionDeleteCommand.Process(writeModel, _repository, _validationService, _unitOfWork);
    }
}