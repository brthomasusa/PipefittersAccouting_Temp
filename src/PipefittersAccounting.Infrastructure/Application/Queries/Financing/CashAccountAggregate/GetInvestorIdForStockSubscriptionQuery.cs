using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetInvestorIdForStockSubscriptionQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetInvestorIdForStockSubscriptionParameter queryParameters,
                                                                                                   DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID
                FROM Finance.StockSubscriptions sub
                LEFT JOIN Finance.Financiers fin ON fin.FinancierID = sub.FinancierId
                WHERE sub.StockId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.StockId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    Guid investorID = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
                    return OperationResult<Guid>.CreateSuccessResult(investorID);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex.Message);
            }
        }
    }
}