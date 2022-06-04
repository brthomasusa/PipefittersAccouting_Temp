using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class VerifyInvestorIdentificationQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetInvestorIdentificationParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    FinancierId
                FROM Finance.Financiers
                WHERE FinancierId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.FinancierId, DbType.Guid);


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