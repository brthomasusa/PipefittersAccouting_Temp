using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetEconomicEventIdentificationInfoQuery
    {
        public async static Task<OperationResult<EconomicEventIdentificationInfo>> Query(EconomicEventIdentificationParameters queryParameters,
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
                    EconomicEventIdentificationInfo eventInfo =
                        await connection.QueryFirstOrDefaultAsync<EconomicEventIdentificationInfo>(sql, parameters);

                    if (eventInfo is null)
                    {
                        string msg = $"Unable to locate an economic event with Id '{queryParameters.EventId}'!";
                        return OperationResult<EconomicEventIdentificationInfo>.CreateFailure(msg);
                    }

                    return OperationResult<EconomicEventIdentificationInfo>.CreateSuccessResult(eventInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<EconomicEventIdentificationInfo>.CreateFailure(ex.Message);
            }
        }
    }
}