#pragma warning disable CS8619

using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeListItemsQueryDapper
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<EmployeeListItem>>> Query(GetEmployees queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    EmployeeId,
                    CONCAT(FirstName,' ',COALESCE(MiddleInitial,''),' ',LastName) as EmployeeFullName,
                    Telephone,
                    IsActive,
                    IsSupervisor 
                FROM HumanResources.Employees          
                ORDER BY LastName
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = "SELECT COUNT(EmployeeId) FROM HumanResources.Employees";

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<EmployeeListItem>(sql, parameters);
                    var count = await connection.ExecuteScalarAsync<int>(totalRecordsSql);
                    var pagedList = PagedList<EmployeeListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);
                    return OperationResult<PagedList<EmployeeListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<EmployeeListItem>>.CreateFailure(ex);
            }
        }
    }
}