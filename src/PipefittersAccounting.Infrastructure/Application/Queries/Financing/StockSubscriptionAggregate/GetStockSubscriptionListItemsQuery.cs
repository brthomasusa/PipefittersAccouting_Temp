using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class GetStockSubscriptionListItemsQuery
    {
        public async static Task<OperationResult<PagedList<StockSubscriptionListItem>>> Query(GetStockSubscriptionListItemParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    subs.StockId, fin.FinancierName AS InvestorName,
                    fin.ContactFirstName + ' ' + ISNULL(fin.ContactMiddleInitial, '') + ' ' + fin.ContactLastName AS ContactName,
                    fin.ContactTelephone,  
                    subs.StockIssueDate, subs.SharesIssured, subs.PricePerShare
                FROM Finance.StockSubscriptions subs
                JOIN Finance.Financiers fin ON subs.FinancierId = fin.FinancierID
                ORDER BY fin.FinancierName, subs.StockIssueDate
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(StockId) FROM Finance.StockSubscriptions";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql);
                    var items = await connection.QueryAsync<StockSubscriptionListItem>(sql, parameters);
                    var pagedList = PagedList<StockSubscriptionListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<StockSubscriptionListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }

        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;
    }
}