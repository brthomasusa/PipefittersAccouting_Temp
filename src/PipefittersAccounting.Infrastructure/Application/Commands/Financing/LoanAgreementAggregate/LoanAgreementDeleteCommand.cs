using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.LoanAgreementAggregate
{
    public class LoanAgreementDeleteCommand : WriteCommandHandler<LoanAgreementWriteModel,
                                                                  ILoanAgreementAggregateRepository,
                                                                  ILoanAgreementValidationService,
                                                                  LoanAgreement>
    {
        public LoanAgreementDeleteCommand
        (
            LoanAgreementWriteModel model,
            ILoanAgreementAggregateRepository repo,
            ILoanAgreementValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(model, repo, validationService, unitOfWork)
        {

        }

        protected override async Task<ValidationResult> Validate()
        {
            return await ValidationService.IsValidDeleteLoanAgreementInfo(WriteModel);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            OperationResult<LoanAgreement> result = await Repository.GetByIdAsync(WriteModel.LoanId);

            if (!result.Success)
            {
                return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
            }

            try
            {
                Repository.Delete(result.Result);
                await UnitOfWork.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}