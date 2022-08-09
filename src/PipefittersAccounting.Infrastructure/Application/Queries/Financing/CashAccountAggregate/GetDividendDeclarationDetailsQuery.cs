using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;


namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetDividendDeclarationDetailsQuery
    {
        public async static Task<OperationResult<DividendDeclarationDetails>> Query(GetDividendDeclarationParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    sub.StockId, 
                    sub.StockIssueDate, 
                    sub.SharesIssured, 
                    sub.PricePerShare,    
                    dividend.DividendId, 
                    dividend.DividendDeclarationDate,
                    dividend.DividendPerShare,    
                    cash.CashAcctTransactionDate AS DatePaid, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid,
                    dividend.CreatedDate,
                    dividend.LastModifiedDate
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.StockSubscriptions sub ON fin.FinancierID = sub.FinancierId
                LEFT JOIN Finance.DividendDeclarations dividend ON sub.StockId = dividend.StockId
                LEFT JOIN CashManagement.CashTransactions cash ON dividend.DividendId = cash.EventID
                WHERE dividend.DividendId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.DividendId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    DividendDeclarationDetails detail = await connection.QueryFirstOrDefaultAsync<DividendDeclarationDetails>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a stock subscription with StockId '{queryParameters.DividendId}'!";
                        return OperationResult<DividendDeclarationDetails>.CreateFailure(msg);
                    }

                    return OperationResult<DividendDeclarationDetails>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<DividendDeclarationDetails>.CreateFailure(ex.Message);
            }
        }
    }
}