using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class VerifyCashDisbursementDividendPaymentQuery
    {
        public async static Task<OperationResult<VerifyCashDisbursementForDividendPayment>> Query(GetDividendDeclarationParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    sub.StockId, 
                    dividend.DividendId, 
                    dividend.DividendDeclarationDate,
                    CASE
                        WHEN dividend.DividendPerShare IS NULL THEN 0        
                        ELSE dividend.DividendPerShare
                    END AS DividendDeclared,     
                    cash.CashAcctTransactionDate AS DatePaid, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.StockSubscriptions sub ON fin.FinancierID = sub.FinancierId
                LEFT JOIN Finance.DividendDeclarations dividend ON sub.StockId = dividend.StockId
                LEFT JOIN CashManagement.CashTransactions cash ON dividend.DividendId = cash.EventID
                WHERE dividend.DividendId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.DividendId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    VerifyCashDisbursementForDividendPayment detail =
                        await connection.QueryFirstOrDefaultAsync<VerifyCashDisbursementForDividendPayment>(sql, parameters);

                    if (detail is null)
                    {
                        string msg = "Verification failed using the provided stock subscription id and investor indentification!";
                        return OperationResult<VerifyCashDisbursementForDividendPayment>.CreateFailure(msg);
                    }

                    return OperationResult<VerifyCashDisbursementForDividendPayment>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<VerifyCashDisbursementForDividendPayment>.CreateFailure(ex.Message);
            }
        }
    }
}