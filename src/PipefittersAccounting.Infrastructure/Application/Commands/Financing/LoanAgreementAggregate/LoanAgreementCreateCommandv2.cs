using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.LoanAgreementAggregate
{
    public class LoanAgreementCreateCommandv2 : WriteCommandHandler<LoanAgreementWriteModel,
                                                                   ILoanAgreementAggregateRepository,
                                                                   ILoanAgreementValidationService,
                                                                   LoanAgreement>
    {
        public LoanAgreementCreateCommandv2
        (
            LoanAgreementWriteModel model,
            ILoanAgreementAggregateRepository repo,
            ILoanAgreementValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(model, repo, validationService, unitOfWork)
        {

        }

        protected override async Task<ValidationResult> Validate(LoanAgreementWriteModel writeModel,
                                                                 ILoanAgreementValidationService validationService)
        {
            return await validationService.IsValidCreateLoanAgreementInfo(writeModel);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand(LoanAgreementWriteModel writeModel,
                                                                            ILoanAgreementAggregateRepository repository,
                                                                            IUnitOfWork unitOfWork)
        {
            LoanAgreement agreement = new
            (
                LoanAgreementEconEvent.Create(EntityGuidID.Create(writeModel.LoanId)),
                EntityGuidID.Create(writeModel.FinancierId),
                LoanAmount.Create(writeModel.LoanAmount),
                InterestRate.Create(writeModel.InterestRate),
                LoanDate.Create(writeModel.LoanDate),
                MaturityDate.Create(writeModel.MaturityDate),
                NumberOfInstallments.Create(writeModel.NumberOfInstallments),
                EntityGuidID.Create(writeModel.UserId),
                ConvertToLoanInstallmentList(writeModel.AmortizationSchedule)
            );

            await repository.AddAsync(agreement);
            await unitOfWork.Commit();
            return OperationResult<bool>.CreateSuccessResult(true);
        }

        private List<LoanInstallment> ConvertToLoanInstallmentList(List<LoanInstallmentWriteModel> models)
        {
            List<LoanInstallment> installments = new();

            models.ForEach(x =>
                installments.Add
                (
                    new LoanInstallment
                    (
                        LoanPaymentEconEvent.Create(EntityGuidID.Create(x.LoanInstallmentId)),
                        EntityGuidID.Create(x.LoanId),
                        InstallmentNumber.Create(x.InstallmentNumber),
                        PaymentDueDate.Create(x.PaymentDueDate),
                        EqualMonthlyInstallment.Create(x.EqualMonthlyInstallment),
                        LoanPrincipalAmount.Create(x.LoanPrincipalAmount),
                        LoanInterestAmount.Create(x.LoanInterestAmount),
                        LoanPrincipalRemaining.Create(x.LoanPrincipalRemaining),
                        EntityGuidID.Create(x.UserId)
                    )
                )
            );

            return installments;
        }
    }
}