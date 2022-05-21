using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    // Facade pattern; hides the Commands that do the actual work

    public class CashAccountApplicationService : ICashAccountApplicationService
    {
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CashAccountApplicationService
        (
            ICashAccountAggregateValidationService validationService,
            ICashAccountAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            _validationService = validationService;
            _repository = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateCashAccount(CreateCashAccountInfo writeModel)
            => await CashAccountCreateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> UpdateCashAccount(EditCashAccountInfo writeModel)
            => await CashAccountUpdateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteCashAccount(DeleteCashAccountInfo writeModel)
            => await CashAccountDeleteCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateCashDeposit(CreateCashAccountTransactionInfo writeModel)
        => await CashDepositCreateCommandDispatcher.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateCashDisbursement(CreateCashAccountTransactionInfo writeModel)
            => await CashDisbursementCreateCommandDispatcher.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateCashTransfer(CreateCashAccountTransferInfo writeModel)
            => await CashAccountTransferCreateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);
    }
}