using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class GetDividendDeclarationListItemsQuery
    {
        public async static Task<OperationResult<PagedList<DividendDeclarationListItem>>> Query(GetDividendDeclarationsParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    sub.StockId,     
                    dividend.DividendId, 
                    dividend.DividendDeclarationDate,
                    dividend.DividendPerShare,
                    sub.SharesIssured * dividend.DividendPerShare AS DividendPayable,    
                    cash.CashAcctTransactionDate AS DatePaid, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.StockSubscriptions sub ON fin.FinancierID = sub.FinancierId
                LEFT JOIN Finance.DividendDeclarations dividend ON sub.StockId = dividend.StockId
                LEFT JOIN Finance.CashAccountTransactions cash ON dividend.DividendId = cash.EventID
                WHERE sub.StockId = @ID
                ORDER BY dividend.DividendDeclarationDate
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.StockId, DbType.Guid);
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(DividendId) FROM Finance.DividendDeclarations WHERE StockId = @ID";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql, parameters);
                    var items = await connection.QueryAsync<DividendDeclarationListItem>(sql, parameters);
                    var pagedList = PagedList<DividendDeclarationListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<DividendDeclarationListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<DividendDeclarationListItem>>.CreateFailure(ex.Message);
            }
        }

        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;
    }
}