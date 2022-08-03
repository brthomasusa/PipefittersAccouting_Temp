#pragma warning disable CS8619

using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeManagersQuery
    {
        public async static Task<OperationResult<List<EmployeeManager>>> Query(GetEmployeeManagersParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    ee.EmployeeId AS ManagerId,
                    CONCAT(ee.FirstName,' ',COALESCE(ee.MiddleInitial,''),' ',ee.LastName) as ManagerFullName,
                    CASE
                        WHEN types.EmployeeTypeName = 'Salesperson' THEN 'Sales'        
                        WHEN types.EmployeeTypeName = 'Maintenance' THEN 'Maintenance'
                        WHEN types.EmployeeTypeName = 'Materials Handler' THEN 'Warehouse'
                        WHEN types.EmployeeTypeName = 'Purchasing Agent' THEN 'Purchasing'
                        WHEN types.EmployeeTypeName = 'Accountant' THEN 'Accounting'
                        WHEN types.EmployeeTypeName = 'Administrator' THEN 'Administrators'
                    END AS [Group]              
                FROM HumanResources.Employees ee
                JOIN HumanResources.EmployeeTypes types ON ee.EmployeeTypeId = types.EmployeeTypeId
                WHERE ee.IsSupervisor = 1
                ORDER BY ee.LastName, ee.FirstName";

                using (var connection = ctx.CreateConnection())
                {
                    var managers = await connection.QueryAsync<EmployeeManager>(sql);
                    return OperationResult<List<EmployeeManager>>.CreateSuccessResult(managers.ToList());
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<EmployeeManager>>.CreateFailure(ex.Message);
            }
        }
    }
}