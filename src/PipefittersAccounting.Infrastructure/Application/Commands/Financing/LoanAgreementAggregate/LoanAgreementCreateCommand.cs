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
    public class LoanAgreementCreateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            LoanAgreementWriteModel model,
            ILoanAgreementAggregateRepository repo,
            ILoanAgreementValidationService validationService,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                OperationResult<bool> exist = await repo.Exists(model.LoanId);
                if (exist.Result)
                {
                    string errMsg = $"Create loan agreement failed! A loan agreement with this Id: {model.LoanId} already exists!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                ValidationResult validationResult = await validationService.IsValidCreateLoanAgreementInfo(model);

                if (validationResult.IsValid)
                {
                    LoanAgreement agreement = new
                    (
                        LoanAgreementEconEvent.Create(EntityGuidID.Create(model.LoanId)),
                        EntityGuidID.Create(model.FinancierId),
                        LoanAmount.Create(model.LoanAmount),
                        InterestRate.Create(model.InterestRate),
                        LoanDate.Create(model.LoanDate),
                        MaturityDate.Create(model.MaturityDate),
                        NumberOfInstallments.Create(model.NumberOfInstallments),
                        EntityGuidID.Create(model.UserId),
                        ConvertToLoanInstallmentList(model.AmortizationSchedule)
                    );

                    await repo.AddAsync(agreement);
                    await unitOfWork.Commit();

                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }

        private static List<LoanInstallment> ConvertToLoanInstallmentList(List<LoanInstallmentWriteModel> models)
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