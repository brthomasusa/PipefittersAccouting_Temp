#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    // Verify that a financier and a dividend declaration are known to the system  
    // and that the financier is associated with this particular declaration and 
    // should receive payment for it.

    public class VerifyInvestorHasDividendDeclarationRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyInvestorHasDividendDeclarationRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();
            // Task<OperationResult<Guid>> GetInvestorIdForDividendDeclaration(GetDividendDeclarationParameter queryParameters)
            GetDividendDeclarationParameter queryParameters =
                new() { DividendId = transaction.EventId };

            OperationResult<Guid> result =
                await _cashAcctQrySvc.GetInvestorIdForDividendDeclaration(queryParameters);

            if (result.Success)
            {
                if (result.Result == transaction.AgentId)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transaction);
                    }
                }
                else
                {
                    // The dividend id and financier id have been validated.
                    // So, to be here means the dividend declaration is known to the
                    // sytem but is not owed to this financier.
                    string msg = "Invalid dividend declaration <--> investor combo! This dividend declaration is not owed to this investor";
                    validationResult.Messages.Add(msg);
                }

            }
            else
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}