using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class VerifyEmployeeSupervisorLinkQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetEmployeeParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    SupervisorId
                FROM HumanResources.Employees        
                WHERE EmployeeId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.EmployeeID, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    Guid supvID = await connection.QueryFirstOrDefaultAsync<Guid>(sql, parameters);
                    return OperationResult<Guid>.CreateSuccessResult(supvID);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex.Message);
            }
        }
    }
}