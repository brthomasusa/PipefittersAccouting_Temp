using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate
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

        public async Task<OperationResult<bool>> EditStockSubscription(StockSubscriptionWriteModel writeModel)
            => await StockSubscriptionEditCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteStockSubscription(StockSubscriptionWriteModel writeModel)
            => await StockSubscriptionDeleteCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateDividendDeclaration(DividendDeclarationWriteModel writeModel)
            => await DividendDeclarationCreateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> EditDividendDeclaration(DividendDeclarationWriteModel writeModel)
            => await DividendDeclarationEditCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteDividendDeclaration(DividendDeclarationWriteModel writeModel)
            => await DividendDeclarationDeleteCommand.Process(writeModel, _repository, _validationService, _unitOfWork);
    }
}