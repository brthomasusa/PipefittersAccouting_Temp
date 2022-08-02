using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeDetailQuery
    {
        public async static Task<OperationResult<EmployeeDetail>> Query(GetEmployeeParameter queryParameters, DapperContext ctx)
        {
            try
            {
                if (await IsValidEmployeeID(queryParameters.EmployeeID, ctx) == false)
                {
                    string errMsg = $"No employee record found where EmployeeId equals '{queryParameters.EmployeeID}'.";
                    return OperationResult<EmployeeDetail>.CreateFailure(errMsg);
                }

                var sql =
                @"SELECT 
                    ee.EmployeeId, ee.SupervisorId, types.EmployeeTypeId, types.EmployeeTypeName, 
                    CONCAT(supv.FirstName,' ',COALESCE(supv.MiddleInitial,''),' ',supv.LastName) as ManagerFullName,
                    ee.LastName, ee.FirstName, ee.MiddleInitial, 
                    CONCAT(ee.FirstName,' ',COALESCE(ee.MiddleInitial,''),' ',ee.LastName) as EmployeeFullName, 
                    ee.SSN, ee.Telephone, ee.AddressLine1, ee.AddressLine2, ee.City, ee.StateCode, ee.Zipcode,                                
                    ee.MaritalStatus, ee.Exemptions, ee.PayRate, ee.StartDate, ee.IsActive, ee.IsSupervisor, ee.CreatedDate, ee.LastModifiedDate                
                FROM HumanResources.Employees ee
                INNER JOIN
                (
                    SELECT 
                        EmployeeId, LastName, FirstName, MiddleInitial 
                    FROM HumanResources.Employees supv
                    WHERE IsSupervisor = 1
                ) supv ON ee.SupervisorId = supv.EmployeeId
                JOIN HumanResources.EmployeeTypes types ON ee.EmployeeTypeId = types.EmployeeTypeId       
                WHERE ee.EmployeeId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.EmployeeID, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    EmployeeDetail detail = await connection.QueryFirstOrDefaultAsync<EmployeeDetail>(sql, parameters);
                    return OperationResult<EmployeeDetail>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<EmployeeDetail>.CreateFailure(ex.Message);
            }
        }

        private async static Task<bool> IsValidEmployeeID(Guid employeeId, DapperContext ctx)
        {
            string sql = $"SELECT EmployeeID FROM HumanResources.Employees WHERE EmployeeId = @ID";
            var parameters = new DynamicParameters();
            parameters.Add("ID", employeeId, DbType.Guid);

            using (var connection = ctx.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync(sql, parameters);
                return result != null;
            }
        }
    }
}