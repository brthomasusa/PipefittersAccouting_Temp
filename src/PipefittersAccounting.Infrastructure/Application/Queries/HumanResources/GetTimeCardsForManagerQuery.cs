using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Queries.HumanResources
{
    public class GetTimeCardsForManagerQuery
    {
        public async static Task<OperationResult<List<TimeCardWithPymtInfo>>> Query(GetTimeCardsForManagerParameter queryParams, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    cards.TimeCardId, cards.EmployeeId, cards.SupervisorId, 
                    CONCAT(ee.FirstName,' ',COALESCE(ee.MiddleInitial,''),' ',ee.LastName) as EmployeeFullName,                            
                    cards.PayPeriodEnded, cards.RegularHours, cards.OverTimeHours, 
                    cash.CashAcctTransactionDate AS DatePaid, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid,    
                    cards.UserId
                FROM HumanResources.TimeCards cards
                JOIN HumanResources.Employees ee ON cards.EmployeeId = ee.EmployeeId
                LEFT JOIN Finance.CashAccountTransactions cash ON cards.TimeCardId = cash.EventID       
                WHERE cards.SupervisorId = @ID AND cards.PayPeriodEnded = @PERIODENDDATE
                ORDER BY cards.PayPeriodEnded, ee.LastName, ee.FirstName";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParams.SupervisorId, DbType.Guid);
                parameters.Add("PERIODENDDATE", queryParams.PayPeriodEndDate, DbType.DateTime2);

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