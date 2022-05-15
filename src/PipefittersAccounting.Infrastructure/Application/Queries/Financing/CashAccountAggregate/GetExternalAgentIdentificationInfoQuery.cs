using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetExternalAgentIdentificationInfoQuery
    {
        public async static Task<OperationResult<ExternalAgentIdentificationInfo>> Query(ExternalAgentIdentificationParameters queryParameters,
                                                                                         DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    agents.AgentId, agents.AgentTypeId, types.AgentTypeName
                FROM Shared.ExternalAgents agents
                LEFT JOIN Shared.ExternalAgentTypes types ON agents.AgentTypeId = types.AgentTypeId
                WHERE agents.AgentId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.AgentId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    ExternalAgentIdentificationInfo agentInfo =
                        await connection.QueryFirstOrDefaultAsync<ExternalAgentIdentificationInfo>(sql, parameters);

                    if (agentInfo is null)
                    {
                        string msg = $"Unable to locate an external agent with Id '{queryParameters.AgentId}'!";
                        return OperationResult<ExternalAgentIdentificationInfo>.CreateFailure(msg);
                    }

                    return OperationResult<ExternalAgentIdentificationInfo>.CreateSuccessResult(agentInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<ExternalAgentIdentificationInfo>.CreateFailure(ex.Message);
            }
        }
    }
}