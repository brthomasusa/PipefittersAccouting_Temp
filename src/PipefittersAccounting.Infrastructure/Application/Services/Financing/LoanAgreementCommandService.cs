using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class LoanAgreementCommandService : ILoanAgreementCommandService
    {
        private readonly ILoanAgreementAggregateRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public LoanAgreementCommandService(ILoanAgreementAggregateRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> CreateLoanAgreement(CreateLoanAgreementInfo writeModel)
            => await LoanAgreementCreateCommand.Execute(writeModel, _repo, _unitOfWork);

        public async Task<OperationResult<bool>> DeleteLoanAgreement(DeleteLoanAgreementInfo writeModel)
            => await LoanAgreementDeleteCommand.Execute(writeModel, _repo, _unitOfWork);
    }
}