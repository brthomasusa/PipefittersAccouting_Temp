using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class VerifyStockSubscriptionIsUniqueQuery
    {
        public async static Task<OperationResult<Guid>> Query(UniqueStockSubscriptionParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    StockId
                FROM Finance.StockSubscriptions
                WHERE FinancierId = @FINANCIERID AND 
                    StockIssueDate = @STOCKISSUEDATE AND 
                    SharesIssured = @SHARESISSUED AND 
                    PricePerShare = @PRICEPERSHARE";

                var parameters = new DynamicParameters();
                parameters.Add("FINANCIERID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("STOCKISSUEDATE", queryParameters.StockIssueDate, DbType.DateTime2);
                parameters.Add("SHARESISSUED", queryParameters.SharesIssued, DbType.Int32);
                parameters.Add("PRICEPERSHARE", queryParameters.PricePerShare, DbType.Decimal);

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