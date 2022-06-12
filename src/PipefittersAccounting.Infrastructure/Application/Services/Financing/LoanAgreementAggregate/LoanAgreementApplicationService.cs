using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate
{
    public class LoanAgreementApplicationService : ILoanAgreementApplicationService
    {
        private readonly ILoanAgreementValidationService _validationService;
        private readonly ILoanAgreementAggregateRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public LoanAgreementApplicationService
        (
            ILoanAgreementValidationService validationService,
            ILoanAgreementAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            _validationService = validationService;
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateLoanAgreement(LoanAgreementWriteModel writeModel)
        {
            return await LoanAgreementCreateCommand.Process(writeModel, _repo, _validationService, _unitOfWork);
        }

        public async Task<OperationResult<bool>> DeleteLoanAgreement(LoanAgreementWriteModel writeModel)
        {
            return await LoanAgreementDeleteCommand.Process(writeModel, _repo, _validationService, _unitOfWork);
        }
    }
}
