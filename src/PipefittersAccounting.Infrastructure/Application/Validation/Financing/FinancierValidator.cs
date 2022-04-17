#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    // Verify that a financier is known to the system before 
    // receiving funds from or sending funds to.
    public class FinancierValidator : ICashTransactionValidator
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public FinancierValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public ICashTransactionValidator? Next { get; set; }

        public async Task<ValidationResult> Validate(CashTransaction transaction)
        {
            ValidationResult validationResult = new();

            FinancierIdValidationParams financierParam =
                new() { FinancierId = transaction.AgentId };

            OperationResult<FinancierIdValidationModel> financierResult =
                await _cashAcctQrySvc.GetFinancierIdValidationModel(financierParam);

            if (financierResult.Success)
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next?.Validate(transaction);
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