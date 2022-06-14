using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class VerifyEmployeeSSNIsUniqueQuery
    {
        public async static Task<OperationResult<Guid>> Query(UniqueEmployeSSNParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    EmployeeId
                FROM HumanResources.Employees        
                WHERE SSN = @SSN";

                var parameters = new DynamicParameters();
                parameters.Add("SSN", queryParameters.SSN, DbType.String);

                using (var connection = ctx.CreateConnection())
                {
                    Guid employeeID = await connection.QueryFirstOrDefaultAsync<Guid>(sql, parameters);
                    return OperationResult<Guid>.CreateSuccessResult(employeeID);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex.Message);
            }
        }
    }
}