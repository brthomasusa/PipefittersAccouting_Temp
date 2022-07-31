#pragma warning disable CS8619

using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetEmployeeTypesQuery
    {
        public async static Task<OperationResult<List<EmployeeTypes>>> Query(GetEmployeeTypesParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    EmployeeTypeId, 
                    EmployeeTypeName 
                FROM HumanResources.EmployeeTypes
                ORDER BY EmployeeTypeName";

                using (var connection = ctx.CreateConnection())
                {
                    var employeeTypes = await connection.QueryAsync<EmployeeTypes>(sql);
                    return OperationResult<List<EmployeeTypes>>.CreateSuccessResult(employeeTypes.ToList());
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<EmployeeTypes>>.CreateFailure(ex.Message);
            }
        }
    }
}