using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetFinancierIdValidationModelQuery
    {
        public async static Task<OperationResult<FinancierIdValidationModel>> Query(FinancierIdValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    FinancierID, FinancierName
                FROM Finance.Financiers     
                WHERE FinancierId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.FinancierId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    FinancierIdValidationModel model = await connection.QueryFirstOrDefaultAsync<FinancierIdValidationModel>(sql, parameters);
                    if (model is null)
                    {
                        string msg = $"Unable to locate a financier with FinancierId: {queryParameters.FinancierId}.";
                        return OperationResult<FinancierIdValidationModel>.CreateFailure(msg);
                    }

                    return OperationResult<FinancierIdValidationModel>.CreateSuccessResult(model);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<FinancierIdValidationModel>.CreateFailure(ex.Message);
            }
        }
    }
}