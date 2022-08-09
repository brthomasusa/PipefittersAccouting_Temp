using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.CashManagement
{
    public class GetInvestorIdForDividendDeclarationQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetDividendDeclarationParameter queryParameters,
                                                              DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID 
                FROM Finance.DividendDeclarations divd
                LEFT JOIN Finance.StockSubscriptions sub ON divd.StockId = sub.StockId
                LEFT JOIN Finance.Financiers fin ON sub.FinancierId = fin.FinancierID
                WHERE divd.DividendId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.DividendId, DbType.Guid);

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