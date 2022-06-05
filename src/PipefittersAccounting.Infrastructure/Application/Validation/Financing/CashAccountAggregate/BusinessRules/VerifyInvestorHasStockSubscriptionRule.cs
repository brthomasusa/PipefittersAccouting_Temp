#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules
{
    // Verify that a financier and a stock subscription are known to the system  
    // and that the financier is associated with this particular stock subscription.

    public class VerifyInvestorHasStockSubscriptionRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyInvestorHasStockSubscriptionRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            CreditorLoanAgreementValidationParameters queryParameters = new() { LoanId = transaction.EventId, FinancierId = transaction.AgentId };

            OperationResult<CreditorIssuedLoanAgreementValidationInfo> result =
                await _cashAcctQrySvc.GetCreditorIssuedLoanAgreementValidationInfo(queryParameters);

            if (result.Success)
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next?.Validate(transaction);
                }
            }
            else
            // The stock id and financier id have been validated.
            // So, to be here means the stock subscription is known to the
            // sytem but was not issued by this financier.
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}