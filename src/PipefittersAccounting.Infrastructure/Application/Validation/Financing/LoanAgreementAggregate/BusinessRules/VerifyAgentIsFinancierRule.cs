#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules
{
    public class VerifyAgentIsFinancierRule : BusinessRule<LoanAgreementWriteModel>
    {
        private readonly ISharedQueryService _qrySvc;

        public VerifyAgentIsFinancierRule(ISharedQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(LoanAgreementWriteModel loanAgreement)
        {
            ValidationResult validationResult = new();

            AgentIdentificationParameter queryParameters = new() { AgentId = loanAgreement.FinancierId };
            OperationResult<AgentIdentificationInfo> agentResult =
                await _qrySvc.GetExternalAgentIdentificationInfo(queryParameters);

            // Is the agent id known to the system?
            if (agentResult.Success)
            {
                // Does the agent id represent a creditor?
                if ((AgentTypeEnum)agentResult.Result.AgentTypeId == AgentTypeEnum.Financier)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(loanAgreement);
                    }
                }
                else
                {
                    string msg = $"An agent of type '{agentResult.Result.AgentTypeName}' is not valid for this operation. Expecting a creditor!";
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