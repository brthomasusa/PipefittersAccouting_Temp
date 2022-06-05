using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Shared
{
    public class GetEventIdentificationInfoQuery
    {
        public async static Task<OperationResult<EventIdentificationInfo>> Query(EventIdentificationParameter queryParameters,
                                                                                         DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    evt.EventId, evt.EventTypeId, types.EventTypeName
                FROM Shared.EconomicEvents evt
                LEFT JOIN Shared.EconomicEventTypes types ON evt.EventTypeId = types.EventTypeId
                WHERE evt.EventId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.EventId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    EventIdentificationInfo eventInfo =
                        await connection.QueryFirstOrDefaultAsync<EventIdentificationInfo>(sql, parameters);

                    if (eventInfo is null)
                    {
                        string msg = $"Unable to locate an economic event with Id '{queryParameters.EventId}'!";
                        return OperationResult<EventIdentificationInfo>.CreateFailure(msg);
                    }

                    return OperationResult<EventIdentificationInfo>.CreateSuccessResult(eventInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<EventIdentificationInfo>.CreateFailure(ex.Message);
            }
        }
    }
}