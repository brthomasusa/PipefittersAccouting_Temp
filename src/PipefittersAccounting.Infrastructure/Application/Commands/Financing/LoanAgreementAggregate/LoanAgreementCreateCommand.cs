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
    public class LoanAgreementCreateCommand : WriteCommandHandler<LoanAgreementWriteModel,
                                                                  ILoanAgreementAggregateRepository,
                                                                  ILoanAgreementValidationService,
                                                                  LoanAgreement>
    {
        public LoanAgreementCreateCommand
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
            return await ValidationService.IsValidCreateLoanAgreement(WriteModel);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            try
            {
                LoanAgreement agreement = new
                (
                    LoanAgreementEconEvent.Create(EntityGuidID.Create(WriteModel.LoanId)),
                    EntityGuidID.Create(WriteModel.FinancierId),
                    LoanAmount.Create(WriteModel.LoanAmount),
                    InterestRate.Create(WriteModel.InterestRate),
                    LoanDate.Create(WriteModel.LoanDate),
                    MaturityDate.Create(WriteModel.MaturityDate),
                    NumberOfInstallments.Create(WriteModel.NumberOfInstallments),
                    EntityGuidID.Create(WriteModel.UserId),
                    ConvertToLoanInstallmentList(WriteModel.AmortizationSchedule)
                );

                await Repository.AddAsync(agreement);
                await UnitOfWork.Commit();
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
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
                        EqualMonthlyInstallment.Create(x.PaymentAmount),
                        LoanPrincipalAmount.Create(x.PrincipalPymtAmount),
                        LoanInterestAmount.Create(x.InterestPymtAmount),
                        LoanPrincipalRemaining.Create(x.PrincipalRemaining),
                        EntityGuidID.Create(x.UserId)
                    )
                )
            );

            return installments;
        }
    }
}