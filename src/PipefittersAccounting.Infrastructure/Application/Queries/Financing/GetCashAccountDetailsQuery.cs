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
                        FROM Finance.CashAccounts cashacct
                        -- JOIN Finance.CashAccountTypes acctTypes ON cashacct.CashAccountTypeId = acctTypes.CashAccountTypeId
                        LEFT JOIN Finance.CashAccountTransactions acctTrans ON cashacct.CashAccountId = acctTrans.CashAccountId
                        GROUP BY cashacct.CashAccountId, acctTrans.CashTransactionTypeId    
                    ) AS transactionSummary
                    GROUP BY transactionSummary.CashAccountId
                ) AS acctSummary
                JOIN Finance.CashAccounts cashAcct ON acctSummary.CashAccountId = cashAcct.CashAccountId
                JOIN Finance.CashAccountTypes acctTypes ON cashAcct.CashAccountTypeId = acctTypes.CashAccountTypeId
                WHERE cashAcct.CashAccountId = @ID";

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