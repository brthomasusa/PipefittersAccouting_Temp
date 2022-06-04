using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class VerifyCashDepositOfStockIssueProceedsQuery
    {
        public async static Task<OperationResult<VerificationOfCashDepositStockIssueProceeds>> Query(GetStockSubscriptionParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID, sub.StockId, fin.FinancierName AS InvestorName, sub.StockIssueDate, 
                    sub.SharesIssured, sub.PricePerShare, cash.CashTransactionId,
                    cash.CashAcctTransactionDate AS DateReceived, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountReceived
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.StockSubscriptions sub ON fin.FinancierID = sub.FinancierId
                LEFT JOIN Finance.CashAccountTransactions cash ON sub.StockId = cash.EventID
                WHERE sub.StockId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.StockId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    VerificationOfCashDepositStockIssueProceeds detail =
                        await connection.QueryFirstOrDefaultAsync<VerificationOfCashDepositStockIssueProceeds>(sql, parameters);

                    if (detail is null)
                    {
                        string msg = "Verification failed using the provided stock subscription id!";
                        return OperationResult<VerificationOfCashDepositStockIssueProceeds>.CreateFailure(msg);
                    }

                    return OperationResult<VerificationOfCashDepositStockIssueProceeds>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<VerificationOfCashDepositStockIssueProceeds>.CreateFailure(ex.Message);
            }
        }
    }
}