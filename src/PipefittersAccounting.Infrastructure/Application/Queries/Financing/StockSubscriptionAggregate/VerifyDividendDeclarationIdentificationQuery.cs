using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class VerifyDividendDeclarationIdentificationQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetDividendDeclarationParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    DividendId
                FROM Finance.DividendDeclarations
                WHERE DividendId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.DividendId, DbType.Guid);


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