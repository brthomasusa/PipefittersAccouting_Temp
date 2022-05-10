#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    // Verify that a financier is known to the system before 
    // receiving funds from or sending funds to.
    public class FinancierValidator : ICashTransactionValidator
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public FinancierValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public ICashTransactionValidator? Next { get; set; }

        public async Task<ValidationResult> Validate(CashAccountTransaction deposit)
        {
            ValidationResult validationResult = new();

            FinancierIdValidationParams financierParam =
                new() { FinancierId = (deposit as CashDeposit).Payor.Id };

            OperationResult<FinancierIdValidationModel> financierResult =
                await _cashAcctQrySvc.GetFinancierIdValidationModel(financierParam);

            if (financierResult.Success)
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
                validationResult.Messages.Add(financierResult.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}