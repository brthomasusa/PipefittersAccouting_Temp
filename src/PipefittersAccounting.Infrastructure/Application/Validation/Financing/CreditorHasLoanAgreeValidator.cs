#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class CreditorHasLoanAgreeValidator : ICashTransactionValidator
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public CreditorHasLoanAgreeValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public ICashTransactionValidator? Next { get; set; }

        public async Task<ValidationResult> Validate(CashAccountTransaction deposit)
        {
            ValidationResult validationResult = new();

            CreditorHasLoanAgreeValidationParams loanAgreementParam =
                new() { FinancierId = (deposit as CashDeposit).Payor.Id, LoanId = (deposit as CashDeposit).GoodsOrServiceSold.Id };

            OperationResult<CreditorHasLoanAgreeValidationModel> loanAgreementResult =
                await _cashAcctQrySvc.GetCreditorHasLoanAgreeValidationModel(loanAgreementParam);

            if (loanAgreementResult.Success)
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next?.Validate(deposit);
                }
            }
            else
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add(loanAgreementResult.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}