using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Shared
{
    public class GetAgentIdentificationInfoQuery
    {
        public async static Task<OperationResult<AgentIdentificationInfo>> Query(AgentIdentificationParameter queryParameters,
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
                    AgentIdentificationInfo agentInfo =
                        await connection.QueryFirstOrDefaultAsync<AgentIdentificationInfo>(sql, parameters);

                    if (agentInfo is null)
                    {
                        string msg = $"Unable to locate an external agent with Id '{queryParameters.AgentId}'!";
                        return OperationResult<AgentIdentificationInfo>.CreateFailure(msg);
                    }

                    return OperationResult<AgentIdentificationInfo>.CreateSuccessResult(agentInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<AgentIdentificationInfo>.CreateFailure(ex.Message);
            }
        }
    }
}