#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    // Verify that a financier is known to the system before 
    // receiving funds from or sending funds to.
    public class VerifyAgentIsFinancierRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ISharedQueryService _qrySvc;

        public VerifyAgentIsFinancierRule(ISharedQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transactionInfo)
        {
            ValidationResult validationResult = new();

            ExternalAgentParameter queryParameters = new() { AgentId = transactionInfo.AgentId };
            OperationResult<ExternalAgentReadModel> agentResult =
                await _qrySvc.GetExternalAgentIdentificationInfo(queryParameters);

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