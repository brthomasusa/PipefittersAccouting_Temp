using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.Infrastructure.Application.Queries.CashManagement
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
                    cashAcct.CashAccountId, acctTypes.CashAccountTypeName AS AccountType, cashAcct.BankName, 
                    cashAcct.AccountName, cashAcct.AccountNumber, cashAcct.RoutingTransitNumber, cashAcct.DateOpened, 
                    cashAcct.UserId, acctSummary.Inflow, acctSummary.Outflow, acctSummary.Balance
                FROM
                (
                    SELECT  
                        transactionSummary.CashAccountId,
                        sum(transactionSummary.CashInflows) AS Inflow,
                        sum(transactionSummary.CashOutflows) AS Outflow,
                        sum(transactionSummary.CashInflows) - sum(transactionSummary.CashOutflows) AS Balance
                    FROM
                    (
                        SELECT
                            cashacct.CashAccountId,    
                            CASE
                                -- Cash transaction types that cause an inflow of cash into an account
                                WHEN acctTrans.CashTransactionTypeId IN (1,2,3,9,11) AND 
                                    SUM(acctTrans.CashAcctTransactionAmount) IS NOT NULL 
                                THEN SUM(acctTrans.CashAcctTransactionAmount)
                                ELSE 0
                            END AS CashInflows, 
                            CASE
                                -- Cash transaction types that cause an outflow of cash from an account
                                WHEN acctTrans.CashTransactionTypeId IN (4,5,6,7,8,10) AND 
                                    SUM(acctTrans.CashAcctTransactionAmount) IS NOT NULL 
                                THEN SUM(acctTrans.CashAcctTransactionAmount)
                            ELSE 0
                            END AS CashOutflows      
                        FROM CashManagement.CashAccounts cashacct
                        -- JOIN CashManagement.CashAccountTypes acctTypes ON cashacct.CashAccountTypeId = acctTypes.CashAccountTypeId
                        LEFT JOIN CashManagement.CashTransactions acctTrans ON cashacct.CashAccountId = acctTrans.CashAccountId
                        GROUP BY cashacct.CashAccountId, acctTrans.CashTransactionTypeId    
                    ) AS transactionSummary
                    GROUP BY transactionSummary.CashAccountId
                ) AS acctSummary
                JOIN CashManagement.CashAccounts cashAcct ON acctSummary.CashAccountId = cashAcct.CashAccountId
                JOIN CashManagement.CashAccountTypes acctTypes ON cashAcct.CashAccountTypeId = acctTypes.CashAccountTypeId
                ORDER BY acctTypes.CashAccountTypeName, cashAcct.AccountName
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(CashAccountId) FROM CashManagement.CashAccounts";

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