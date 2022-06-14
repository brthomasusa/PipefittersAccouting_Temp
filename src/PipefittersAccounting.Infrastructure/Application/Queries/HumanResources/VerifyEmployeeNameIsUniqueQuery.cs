using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class VerifyEmployeeNameIsUniqueQuery
    {
        public async static Task<OperationResult<Guid>> Query(UniqueEmployeeNameParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    EmployeeId
                FROM HumanResources.Employees        
                WHERE LastName = @LASTNAME AND FirstName = @FIRSTNAME AND MiddleInitial = @MI";

                var parameters = new DynamicParameters();
                parameters.Add("LASTNAME", queryParameters.LastName, DbType.String);
                parameters.Add("FIRSTNAME", queryParameters.FirstName, DbType.String);
                parameters.Add("MI", queryParameters.MiddleInitial, DbType.String);

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