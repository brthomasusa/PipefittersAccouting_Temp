using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeTimeCardDetailQuery
    {
        public async static Task<OperationResult<TimeCardDetail>> Query(GetTimeCardParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    cards.TimeCardId, ee.EmployeeId, 
                    CONCAT(ee.FirstName,' ',COALESCE(ee.MiddleInitial,''),' ',ee.LastName) 
                        as EmployeeFullName,
                    ee.SupervisorId, CONCAT(supv.FirstName,' ',COALESCE(supv.MiddleInitial,''),' ',supv.LastName) 
                        as ManagerFullName,                                                    
                    ee.MaritalStatus, ee.Exemptions, ee.PayRate, 
                    ee.StartDate, cards.PayPeriodEnded,
                    cards.RegularHours, cards.OverTimeHours, cards.UserId,
                    cards.CreatedDate, cards.LastModifiedDate                
                FROM HumanResources.TimeCards cards
                JOIN HumanResources.Employees ee ON cards.EmployeeId = ee.EmployeeId
                INNER JOIN
                (
                    SELECT 
                        EmployeeId, LastName, FirstName, MiddleInitial 
                    FROM HumanResources.Employees supv
                    WHERE IsSupervisor = 1
                ) supv ON ee.SupervisorId = supv.EmployeeId       
                WHERE cards.TimeCardId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.TimeCardId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    TimeCardDetail detail = await connection.QueryFirstOrDefaultAsync<TimeCardDetail>(sql, parameters);
                    return OperationResult<TimeCardDetail>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<TimeCardDetail>.CreateFailure(ex.Message);
            }
        }
    }
}