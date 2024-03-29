#pragma warning disable CS8619

using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeListItemsQuery
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployeesParameters queryParams, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    ee.EmployeeId,
                    CONCAT(ee.FirstName,' ',COALESCE(ee.MiddleInitial,''),' ',ee.LastName) as EmployeeFullName,
                    ee.Telephone, ee.IsActive, ee.IsSupervisor,
                    CONCAT(supv.FirstName,' ',COALESCE(supv.MiddleInitial,''),' ',supv.LastName) as ManagerFullName ,
                    ISNULL(cards.TimeCards, 0 ) AS TimeCards             
                FROM HumanResources.Employees ee
                LEFT JOIN
                (
                    SELECT 
                        EmployeeId, LastName, FirstName, MiddleInitial 
                    FROM HumanResources.Employees supv
                    WHERE IsSupervisor = 1
                ) supv ON ee.SupervisorId = supv.EmployeeId
                LEFT JOIN
                (
                    SELECT 
                        cards.EmployeeId, COUNT(cards.TimeCardId) AS TimeCards
                    FROM HumanResources.TimeCards cards
                    GROUP BY cards.EmployeeId
                ) cards ON ee.EmployeeId = cards.EmployeeId
                ORDER BY ee.LastName, ee.FirstName, ee.MiddleInitial
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParams.Page, queryParams.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParams.PageSize, DbType.Int32);

                var totalRecordsSql = "SELECT COUNT(EmployeeId) FROM HumanResources.Employees";

                using var connection = ctx.CreateConnection();

                var count = await connection.ExecuteScalarAsync<int>(totalRecordsSql);
                var items = await connection.QueryAsync<EmployeeListItem>(sql, parameters);
                var pagedList = PagedList<EmployeeListItem>.CreatePagedList(items.ToList(), count, queryParams.Page, queryParams.PageSize);

                return OperationResult<PagedList<EmployeeListItem>>.CreateSuccessResult(pagedList);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<EmployeeListItem>>.CreateFailure(ex.Message);
            }
        }

        public async static Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployeesByLastNameParameters queryParams, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    ee.EmployeeId,
                    CONCAT(ee.FirstName,' ',COALESCE(ee.MiddleInitial,''),' ',ee.LastName) as EmployeeFullName,
                    ee.Telephone, ee.IsActive, ee.IsSupervisor,
                    CONCAT(supv.FirstName,' ',COALESCE(supv.MiddleInitial,''),' ',supv.LastName) as ManagerFullName               
                FROM HumanResources.Employees ee
                LEFT JOIN
                (
                    SELECT 
                        EmployeeId, LastName, FirstName, MiddleInitial 
                    FROM HumanResources.Employees supv
                    WHERE IsSupervisor = 1
                ) supv ON ee.SupervisorId = supv.EmployeeId
                WHERE ee.LastName LIKE CONCAT('%',@LastName,'%')
                ORDER BY ee.LastName, ee.FirstName, ee.MiddleInitial
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("LastName", queryParams.LastName, DbType.String);
                parameters.Add("Offset", Offset(queryParams.Page, queryParams.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParams.PageSize, DbType.Int32);

                var totalRecordsSql = "SELECT COUNT(EmployeeId) FROM HumanResources.Employees WHERE LastName LIKE CONCAT('%',@LastName,'%')";

                using var connection = ctx.CreateConnection();

                int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql, parameters);
                var items = await connection.QueryAsync<EmployeeListItem>(sql, parameters);
                var pagedList = PagedList<EmployeeListItem>.CreatePagedList(items.ToList(), count, queryParams.Page, queryParams.PageSize);

                return OperationResult<PagedList<EmployeeListItem>>.CreateSuccessResult(pagedList);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<EmployeeListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}