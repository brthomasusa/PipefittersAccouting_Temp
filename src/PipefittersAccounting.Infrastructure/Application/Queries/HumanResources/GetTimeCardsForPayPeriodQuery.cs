#pragma warning disable CS8619

using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetTimeCardsForPayPeriodQuery
    {
        public async static Task<OperationResult<List<TimeCardWithPymtInfo>>> Query(GetTimeCardsForPayPeriodParameter queryParams, DapperContext ctx)
        {
            try
            {
                var sql = "EXECUTE HumanResources.GetTimeCardInfoForPayPeriod @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParams.UserId, DbType.Guid);

                using var connection = ctx.CreateConnection();

                var items = await connection.QueryAsync<TimeCardWithPymtInfo>(sql, parameters);

                return OperationResult<List<TimeCardWithPymtInfo>>.CreateSuccessResult(items.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<List<TimeCardWithPymtInfo>>.CreateFailure(ex.Message);
            }
        }
    }
}