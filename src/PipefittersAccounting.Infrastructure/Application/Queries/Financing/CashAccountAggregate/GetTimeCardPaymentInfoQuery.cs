#pragma warning disable CS8619

using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetTimeCardPaymentInfoQuery
    {
        public async static Task<OperationResult<List<TimeCardPaymentInfo>>> Query(GetTimeCardPaymentInfoParameter queryParams, DapperContext ctx)
        {
            try
            {
                var sql = "EXECUTE Finance.GetTimeCardPaymentInfo @periodStartDate = @PERIODBEGIN, @periodEndDate = @PERIODENDED";

                var parameters = new DynamicParameters();
                parameters.Add("PERIODBEGIN", queryParams.PayPeriodEnd, DbType.DateTime2);
                parameters.Add("PERIODENDED", queryParams.PayPeriodEnd, DbType.DateTime2);

                using var connection = ctx.CreateConnection();

                var items = await connection.QueryAsync<TimeCardPaymentInfo>(sql, parameters);

                return OperationResult<List<TimeCardPaymentInfo>>.CreateSuccessResult(items.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<List<TimeCardPaymentInfo>>.CreateFailure(ex.Message);
            }
        }
    }
}