#pragma warning disable CS8619

using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeTimeCardListItemsQuery
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<List<TimeCardListItem>>> Query(GetEmployeeParameter queryParams, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    cards.TimeCardId, 
                    CONCAT(supv.FirstName,' ',COALESCE(supv.MiddleInitial,''),' ',supv.LastName) 
                        as ManagerFullName,                                                    
                    cards.PayPeriodEnded, cards.RegularHours, cards.OverTimeHours                                    
                FROM HumanResources.TimeCards cards
                JOIN HumanResources.Employees ee ON cards.EmployeeId = ee.EmployeeId
                INNER JOIN
                (
                    SELECT 
                        EmployeeId, LastName, FirstName, MiddleInitial 
                    FROM HumanResources.Employees supv
                    WHERE IsSupervisor = 1
                ) supv ON ee.SupervisorId = supv.EmployeeId        
                WHERE ee.EmployeeId = 'c40888a1-c182-437e-9c1d-e9227bca7f52'
                ORDER BY cards.PayPeriodEnded";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParams.EmployeeID, DbType.Guid);

                using var connection = ctx.CreateConnection();

                var items = await connection.QueryAsync<TimeCardListItem>(sql, parameters);

                return OperationResult<List<TimeCardListItem>>.CreateSuccessResult(items.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<List<TimeCardListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}