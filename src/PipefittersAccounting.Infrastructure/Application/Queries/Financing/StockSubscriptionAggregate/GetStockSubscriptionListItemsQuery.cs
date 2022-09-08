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
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<StockSubscriptionListItem>>> Query(GetStockSubscriptionListItem queryParameters, DapperContext ctx)
        {
            try
            {
                var sql = StockSubscriptionQueries.SelectAllStockSubscriptionListItems +
                    "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

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

        public async static Task<OperationResult<PagedList<StockSubscriptionListItem>>> Query(GetStockSubscriptionListItemByInvestorName queryParameters, DapperContext ctx)
        {
            try
            {
                var sql = StockSubscriptionQueries.SelectStockSubscriptionListItemsByInvestName +
                    "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("InvestorName", queryParameters.InvestorName, DbType.String);
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<StockSubscriptionListItem>(sql, parameters);
                    int count = items.Count();

                    var pagedList = PagedList<StockSubscriptionListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<StockSubscriptionListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }

        public async static Task<OperationResult<PagedList<StockSubscriptionListItem>>> QueryByFundsRcvd(GetStockSubscriptionListItem queryParameters, DapperContext ctx)
        {
            try
            {
                var sql = StockSubscriptionQueries.SelectStockSubscriptionListItemsByFundsRcvd +
                    "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<StockSubscriptionListItem>(sql, parameters);
                    int count = items.Count();

                    var pagedList = PagedList<StockSubscriptionListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<StockSubscriptionListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }

        public async static Task<OperationResult<PagedList<StockSubscriptionListItem>>> QueryByFundsNotRcvd(GetStockSubscriptionListItem queryParameters, DapperContext ctx)
        {
            try
            {
                var sql = StockSubscriptionQueries.SelectStockSubscriptionListItemsByFundsNotRcvd +
                    "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<StockSubscriptionListItem>(sql, parameters);
                    int count = items.Count();

                    var pagedList = PagedList<StockSubscriptionListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<StockSubscriptionListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}
