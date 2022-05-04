using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing
{
    public class GetCashAccountDetailsQuery
    {
        public async static Task<OperationResult<CashAccountDetail>> Query(GetCashAccount queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    acct_summary.CashAccountId,
                    cashAccts.AccountNumber,
                    cashAccts.BankName,
                    acctTypes.CashAccountTypeName AS AccountType,
                    cashAccts.AccountName,    
                    cashAccts.RoutingTransitNumber AS 'Routing',
                    sum(CashInflows) AS Inflow,
                    sum(CashOutflows) AS Outflow,
                    sum(CashInflows) - sum(CashOutflows) AS Balance
                FROM
                (
                    SELECT
                        trans_summary.CashAccountId,
                        trans_summary.CashInflows, 
                        trans_summary.CashOutflows
                    FROM
                    (
                        SELECT 
                            tt.CashTransactionTypeId, 
                            trans.CashAccountId,
                            CASE
                                -- Cash transaction types that cause an inflow of cash into an account
                                WHEN tt.CashTransactionTypeId IN (1,2,3,9,11) THEN SUM(trans.CashAcctTransactionAmount)
                            END AS CashInflows, 
                            CASE
                                -- Cash transaction types that cause an outflow of cash from an account
                                WHEN tt.CashTransactionTypeId IN (4,5,6,7,8,10) THEN SUM(trans.CashAcctTransactionAmount)
                            END AS CashOutflows        
                        FROM Finance.CashTransactionTypes tt
                        LEFT JOIN Finance.CashAccountTransactions trans ON tt.CashTransactionTypeId = trans.CashTransactionTypeId
                        WHERE trans.CashAccountId = @ID
                        GROUP BY trans.CashAccountId, tt.CashTransactionTypeId
                    ) AS trans_summary
                ) AS acct_summary
                LEFT JOIN Finance.CashAccounts cashAccts ON acct_summary.CashAccountId = cashAccts.CashAccountId
                LEFT JOIN Finance.CashAccountTypes acctTypes ON cashAccts.CashAccountTypeId = acctTypes.CashAccountTypeId
                GROUP BY acct_summary.CashAccountId, cashAccts.AccountNumber, cashAccts.BankName, 
                         acctTypes.CashAccountTypeName, cashAccts.AccountName, cashAccts.RoutingTransitNumber";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.CashAccountId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    CashAccountDetail detail = await connection.QueryFirstOrDefaultAsync<CashAccountDetail>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a cash account with Id '{queryParameters.CashAccountId}'!";
                        return OperationResult<CashAccountDetail>.CreateFailure(msg);
                    }

                    return OperationResult<CashAccountDetail>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CashAccountDetail>.CreateFailure(ex.Message);
            }
        }
    }
}