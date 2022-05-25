#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    // Verify that a financier is known to the system before 
    // receiving funds from or sending funds to.
    public class FinancierAsExternalAgentRule : BusinessRule<CreateCashAccountTransactionInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public FinancierAsExternalAgentRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountTransactionInfo transactionInfo)
        {
            ValidationResult validationResult = new();

            ExternalAgentIdentificationParameters queryParameters = new() { AgentId = transactionInfo.AgentId };
            OperationResult<ExternalAgentIdentificationInfo> agentResult =
                await _cashAcctQrySvc.GetExternalAgentIdentificationInfo(queryParameters);

            // Is the agent id known to the system?
            if (agentResult.Success)
            {
                // Does the agent id represent a financier?
                if ((AgentTypeEnum)agentResult.Result.AgentTypeId == AgentTypeEnum.Financier)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transactionInfo);
                    }
                }
                else
                {
                    string msg = $"An agent of type '{agentResult.Result.AgentTypeName}' is not valid for this operation. Expecting a financier!";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(agentResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}