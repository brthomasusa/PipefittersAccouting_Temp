using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing
{
    public class GetNumberOfCashAccountTransactionsQuery
    {
        public async static Task<OperationResult<int>> Query(GetCashAccount queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    COUNT(trans.CashAccountId) AS numberOfTransactions
                FROM Finance.CashAccounts acct
                JOIN Finance.CashAccountTransactions trans ON acct.CashAccountId = trans.CashAccountId
                WHERE acct.CashAccountId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.CashAccountId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    int numberOfTransactions = await connection.ExecuteScalarAsync<int>(sql, parameters);
                    return OperationResult<int>.CreateSuccessResult(numberOfTransactions);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<int>.CreateFailure(ex.Message);
            }
        }
    }
}