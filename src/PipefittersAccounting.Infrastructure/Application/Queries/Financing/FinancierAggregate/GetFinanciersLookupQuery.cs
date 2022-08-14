using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.FinancierAggregate
{
    public class GetFinanciersLookupQuery
    {
        public async static Task<OperationResult<List<FinancierLookup>>> Query(GetFinanciersLookup queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    FinancierId,
                    FinancierName              
                FROM Finance.Financiers
                ORDER BY FinancierName";

                using (var connection = ctx.CreateConnection())
                {
                    var lookups = await connection.QueryAsync<FinancierLookup>(sql);
                    return OperationResult<List<FinancierLookup>>.CreateSuccessResult(lookups.ToList());
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<FinancierLookup>>.CreateFailure(ex.Message);
            }
        }
    }
}