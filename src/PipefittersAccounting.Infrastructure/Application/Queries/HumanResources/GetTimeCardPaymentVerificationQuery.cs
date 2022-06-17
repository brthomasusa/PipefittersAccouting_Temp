using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetTimeCardPaymentVerificationQuery
    {
        public async static Task<OperationResult<TimeCardPaymentVerification>> Query(GetTimeCardParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    cards.PayPeriodEnded, cards.RegularHours, cards.OverTimeHours,               
                    cash.CashAcctTransactionDate AS DatePaid,
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid     
                FROM HumanResources.TimeCards cards
                LEFT JOIN Finance.CashAccountTransactions cash ON cards.TimeCardId = cash.EventID     
                WHERE cards.TimeCardId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.TimeCardId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    TimeCardPaymentVerification detail = await connection.QueryFirstOrDefaultAsync<TimeCardPaymentVerification>(sql, parameters);
                    return OperationResult<TimeCardPaymentVerification>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<TimeCardPaymentVerification>.CreateFailure(ex.Message);
            }
        }
    }
}