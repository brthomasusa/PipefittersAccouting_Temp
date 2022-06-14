using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetCountOfEmployeeTimeCardsQuery
    {
        public async static Task<OperationResult<int>> Query(GetEmployeeParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    COUNT(cards.TimeCardId) as TimeCards
                FROM HumanResources.Employees ee
                JOIN HumanResources.TimeCards cards ON ee.EmployeeId = cards.EmployeeID       
                WHERE ee.EmployeeId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.EmployeeID, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.QueryFirstOrDefaultAsync<int>(sql, parameters);
                    return OperationResult<int>.CreateSuccessResult(count);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<int>.CreateFailure(ex.Message);
            }
        }
    }
}