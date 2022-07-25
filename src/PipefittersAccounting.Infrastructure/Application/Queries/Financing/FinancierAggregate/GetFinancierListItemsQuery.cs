using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.FinancierAggregate
{
    public class GetFinancierListItemsQuery
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<FinancierListItems>>> Query(GetFinanciers queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT
                    fin.FinancierID, FinancierName, Telephone, 
                    AddressLine1 + ' ' + ISNULL(AddressLine2, '') + ' ' + City + ', ' + StateCode + ' ' + Zipcode AS FullAddress, 
                    ContactFirstName + ' ' + ISNULL(ContactMiddleInitial, '') + ' ' + ContactLastName AS 'ContactFullName',
                    ContactTelephone, IsActive, Loans, Stocks 
                FROM
                (
                    SELECT
                        fin.FinancierID,
                        COUNT(ll.FinancierId) AS Loans,
                        COUNT(ss.FinancierId) AS Stocks 
                    FROM Finance.Financiers fin
                    LEFT JOIN Finance.LoanAgreements ll ON fin.FinancierID = ll.FinancierId
                    LEFT JOIN Finance.StockSubscriptions ss ON fin.FinancierID = ss.FinancierId
                    GROUP BY fin.FinancierID
                ) AS Attachments
                JOIN Finance.Financiers fin ON fin.FinancierID = Attachments.FinancierID
                ORDER BY FinancierName
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(FinancierId) FROM Finance.Financiers";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql);
                    var items = await connection.QueryAsync<FinancierListItems>(sql, parameters);
                    var pagedList = PagedList<FinancierListItems>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<FinancierListItems>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<FinancierListItems>>.CreateFailure(ex.Message);
            }
        }

        public async static Task<OperationResult<PagedList<FinancierListItems>>> Query(GetFinanciersByName queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID, FinancierName, Telephone, 
                    AddressLine1 + ' ' + ISNULL(AddressLine2, '') + ' ' + City + ', ' + StateCode + ' ' + Zipcode AS FullAddress, 
                    ContactFirstName + ' ' + ISNULL(ContactMiddleInitial, '') + ' ' + ContactLastName AS 'ContactFullName',
                    ContactTelephone, IsActive, Loans, Stocks 
                FROM
                (
                    SELECT
                        fin.FinancierID,
                        COUNT(ll.FinancierId) AS Loans,
                        COUNT(ss.FinancierId) AS Stocks 
                    FROM Finance.Financiers fin
                    LEFT JOIN Finance.LoanAgreements ll ON fin.FinancierID = ll.FinancierId
                    LEFT JOIN Finance.StockSubscriptions ss ON fin.FinancierID = ss.FinancierId
                    GROUP BY fin.FinancierID
                ) AS Attachments
                JOIN Finance.Financiers fin ON fin.FinancierID = Attachments.FinancierID
                WHERE FinancierName LIKE CONCAT('%',@Name,'%')
                ORDER BY FinancierName
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Name", queryParameters.Name, DbType.String);
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(FinancierId) FROM Finance.Financiers  WHERE FinancierName LIKE CONCAT('%',@Name,'%')";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql, parameters);
                    var items = await connection.QueryAsync<FinancierListItems>(sql, parameters);
                    var pagedList = PagedList<FinancierListItems>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<FinancierListItems>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<FinancierListItems>>.CreateFailure(ex.Message);
            }
        }
    }
}
