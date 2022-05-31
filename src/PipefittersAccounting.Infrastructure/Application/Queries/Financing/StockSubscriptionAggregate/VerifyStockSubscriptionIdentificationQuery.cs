using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class VerifyStockSubscriptionIdentificationQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetStockSubscriptionParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    StockId
                FROM Finance.StockSubscriptions
                WHERE StockId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.StockId, DbType.Guid);


                using (var connection = ctx.CreateConnection())
                {
                    Guid stockID = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
                    return OperationResult<Guid>.CreateSuccessResult(stockID);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex.Message);
            }
        }
    }
}