using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
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
        {
            CashAccountCreateCommand cmd = new(_validationService);
            return await cmd.Process(writeModel, _repository, _unitOfWork);
        }

        public async Task<OperationResult<bool>> UpdateCashAccount(EditCashAccountInfo writeModel)
        {
            CashAccountUpdateCommand cmd = new(_validationService);
            return await cmd.Process(writeModel, _repository, _unitOfWork);
        }

        public async Task<OperationResult<bool>> DeleteCashAccount(DeleteCashAccountInfo writeModel)
        {
            CashAccountDeleteCommand cmd = new(_validationService);
            return await cmd.Process(writeModel, _repository, _unitOfWork);
        }

        public Task<OperationResult<bool>> CreateCashDeposit(CreateCashDepositInfo writeModel)
            => throw new NotImplementedException();

        public Task<OperationResult<bool>> CreateCashDisbursement(CreateCashDisbursementInfo writeModel)
            => throw new NotImplementedException();
    }
}