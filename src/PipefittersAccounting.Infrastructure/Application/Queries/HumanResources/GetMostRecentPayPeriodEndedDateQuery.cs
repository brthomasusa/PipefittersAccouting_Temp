using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetMostRecentPayPeriodEndedDateQuery
    {
        public async static Task<OperationResult<DateTime>> Query(GetEmployeeParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    MAX (PayPeriodEnded) 
                FROM HumanResources.TimeCards       
                WHERE EmployeeId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.EmployeeID, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    DateTime lastDate = await connection.QueryFirstOrDefaultAsync<DateTime>(sql, parameters);
                    return OperationResult<DateTime>.CreateSuccessResult(lastDate);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<DateTime>.CreateFailure(ex.Message);
            }
        }
    }
}