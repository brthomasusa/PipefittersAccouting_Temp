#pragma warning disable CS8619

using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetPayrollRegisterQuery
    {
        public async static Task<OperationResult<List<PayrollRegister>>> Query(GetPayrollRegisterParameter queryParams, DapperContext ctx)
        {
            try
            {
                var sql = "EXECUTE HumanResources.GetPayrollRegister @PERIODENDED, @PERIODENDED";

                var parameters = new DynamicParameters();
                parameters.Add("PERIODENDED", queryParams.PayPeriodEnded, DbType.DateTime2);

                using var connection = ctx.CreateConnection();

                var items = await connection.QueryAsync<PayrollRegister>(sql, parameters);

                return OperationResult<List<PayrollRegister>>.CreateSuccessResult(items.ToList());
            }
            catch (Exception ex)
            {
                return OperationResult<List<PayrollRegister>>.CreateFailure(ex.Message);
            }
        }
    }
}
