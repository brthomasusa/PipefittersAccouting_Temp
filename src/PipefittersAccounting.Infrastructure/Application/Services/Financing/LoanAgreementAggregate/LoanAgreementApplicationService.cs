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
        private readonly ILoanAgreementAggregateRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public LoanAgreementApplicationService(ILoanAgreementAggregateRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateLoanAgreement(LoanAgreementWriteModel writeModel)
            => await LoanAgreementCreateCommand.Execute(writeModel, _repo, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteLoanAgreement(DeleteLoanAgreementInfo writeModel)
            => await LoanAgreementDeleteCommand.Execute(writeModel, _repo, _unitOfWork);
    }
}