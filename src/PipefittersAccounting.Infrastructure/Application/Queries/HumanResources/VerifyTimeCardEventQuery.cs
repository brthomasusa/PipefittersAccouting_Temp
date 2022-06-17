using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class VerifyTimeCardEventQuery
    {
        public async static Task<OperationResult<TimeCardVerification>> Query(GetTimeCardParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    TimeCardId, EmployeeId, SupervisorId
                FROM HumanResources.TimeCards      
                WHERE cards.TimeCardId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.TimeCardId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    TimeCardVerification detail = await connection.QueryFirstOrDefaultAsync<TimeCardVerification>(sql, parameters);
                    return OperationResult<TimeCardVerification>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<TimeCardVerification>.CreateFailure(ex.Message);
            }
        }
    }
}