#pragma warning disable CS8619

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    EmployeeId AS ManagerId,
                    CONCAT(FirstName,' ',COALESCE(MiddleInitial,''),' ',LastName) as ManagerFullName              
                FROM HumanResources.Employees
                WHERE IsSupervisor = 1
                ORDER BY LastName";

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