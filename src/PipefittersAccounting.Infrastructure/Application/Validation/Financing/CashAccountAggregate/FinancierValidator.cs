#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    // Verify that a financier is known to the system before 
    // receiving funds from or sending funds to.
    public class FinancierAsPayorIdentificationValidator : Validator<CreateCashAccountTransactionInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public FinancierAsPayorIdentificationValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountTransactionInfo transactionInfo)
        {
            ValidationResult validationResult = new();

            FinancierIdValidationParams financierParam =
                new() { FinancierId = transactionInfo.AgentId };

            OperationResult<FinancierIdValidationModel> financierResult =
                await _cashAcctQrySvc.GetFinancierIdValidationModel(financierParam);

            if (financierResult.Success)
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next?.Validate(transactionInfo);
                }
            }
            else
            {
                validationResult.Messages.Add(financierResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}