using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Application.Commands.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;


namespace PipefittersAccounting.Infrastructure.Application.Services.CashManagement
{
    public class CashAccountApplicationService : ICashAccountApplicationService
    {
        private readonly ICashAccountAggregateValidationService _validationService;
        private readonly ICashAccountAggregateRepository _repository;
        private readonly IEmployeePayrollService _payrollService;
        private readonly IUnitOfWork _unitOfWork;

        public CashAccountApplicationService
        (
            ICashAccountAggregateValidationService validationService,
            ICashAccountAggregateRepository repo,
            IEmployeePayrollService payrollService,
            IUnitOfWork unitOfWork
        )
        {
            _validationService = validationService;
            _repository = repo;
            _payrollService = payrollService;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateCashAccount(CashAccountWriteModel writeModel)
            => await CashAccountCreateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> UpdateCashAccount(CashAccountWriteModel writeModel)
            => await CashAccountEditCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteCashAccount(CashAccountWriteModel writeModel)
            => await CashAccountDeleteCommand.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateCashDeposit(CashTransactionWriteModel writeModel)
        => await CashDepositCreateCommandDispatcher.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateCashDisbursement(CashTransactionWriteModel writeModel)
            => await CashDisbursementCreateCommandDispatcher.Process(writeModel, _repository, _validationService, _unitOfWork);

        public async Task<OperationResult<bool>> CreateDisbursementsForPayroll(Guid cashAccountId, Guid userId)
            => await _payrollService.Process(cashAccountId, userId);

        public async Task<OperationResult<bool>> CreateCashTransfer(CashAccountTransferWriteModel writeModel)
            => await CashAccountTransferCreateCommand.Process(writeModel, _repository, _validationService, _unitOfWork);
    }
}