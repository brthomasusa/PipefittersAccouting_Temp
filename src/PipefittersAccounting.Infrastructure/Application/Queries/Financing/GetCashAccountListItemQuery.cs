using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing
{
    public class GetCashAccountListItemQuery
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<CashAccountListItem>>> Query(GetCashAccounts queryParameters, DapperContext ctx)
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
                        WHERE trans.CashAccountId IS NOT NULL
                        GROUP BY trans.CashAccountId, tt.CashTransactionTypeId
                    ) AS trans_summary
                ) AS acct_summary
                LEFT JOIN Finance.CashAccounts cashAccts ON acct_summary.CashAccountId = cashAccts.CashAccountId
                LEFT JOIN Finance.CashAccountTypes acctTypes ON cashAccts.CashAccountTypeId = acctTypes.CashAccountTypeId
                GROUP BY acct_summary.CashAccountId, cashAccts.AccountNumber, cashAccts.BankName, 
                        acctTypes.CashAccountTypeName, cashAccts.AccountName, cashAccts.RoutingTransitNumber
                ORDER BY cashAccts.AccountName
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(CashAccountId) FROM Finance.CashAccounts";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql);
                    var items = await connection.QueryAsync<CashAccountListItem>(sql, parameters);
                    var pagedList = PagedList<CashAccountListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<CashAccountListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<CashAccountListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}