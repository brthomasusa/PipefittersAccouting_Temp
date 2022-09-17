#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Validation.HumanResources.EmployeeAggregate.BusinessRules
{
    public class VerifyEmployeeOnTimeCardIsValidRule : BusinessRule<TimeCardWriteModel>
    {
        private readonly ISharedQueryService _qrySvc;

        public VerifyEmployeeOnTimeCardIsValidRule(ISharedQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(TimeCardWriteModel employee)
        {
            ValidationResult validationResult = new();

            ExternalAgentParameter queryParameters = new() { AgentId = employee.EmployeeId };
            OperationResult<ExternalAgentReadModel> agentResult =
                await _qrySvc.GetExternalAgentIdentificationInfo(queryParameters);

            // Is the agent id known to the system?
            if (agentResult.Success)
            {
                // Does the agent id represent an employee?
                if ((AgentTypeEnum)agentResult.Result.AgentTypeId == AgentTypeEnum.Employee)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(employee);
                    }
                }
                else
                {
                    string msg = $"An agent of type '{agentResult.Result.AgentTypeName}' is not valid for this operation. Expecting an employee!";
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