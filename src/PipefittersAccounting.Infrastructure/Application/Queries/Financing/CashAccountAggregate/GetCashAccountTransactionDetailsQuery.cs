using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetCashAccountTransactionDetailsQuery
    {
        public async static Task<OperationResult<CashAccountTransactionDetail>> Query(GetCashAccountTransactionDetailParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    transctionDetails.CashTransactionId,  transctionDetails.CashAcctTransactionDate,
                    transctionDetails.CashAcctTransactionAmount, transctionDetails.AgentId,
                    aTypes.AgentTypeId, aTypes.AgentTypeName, transctionDetails.EventId,
                    transctionDetails.CashTransactionTypeName, transctionDetails.CheckNumber, 
                    transctionDetails.RemittanceAdvice, transctionDetails.UserId,
                    transctionDetails.CreatedDate, transctionDetails.LastModifiedDate    
                FROM 
                (
                    SELECT 
                        trans.CashTransactionId, trans.CashAccountId, trans.CashTransactionTypeId, transTypes.CashTransactionTypeName,
                        trans.CashAcctTransactionDate, trans.CashAcctTransactionAmount, trans.AgentId, agents.AgentTypeId,
                        trans.EventId, events.EventTypeId, trans.CheckNumber, trans.RemittanceAdvice, trans.UserId,
                        trans.CreatedDate, trans.LastModifiedDate
                    FROM Finance.CashAccountTransactions trans
                    JOIN Finance.CashTransactionTypes transTypes ON trans.CashTransactionTypeId = transTypes.CashTransactionTypeId
                    JOIN Shared.ExternalAgents agents ON trans.AgentId = agents.AgentId
                    JOIN Shared.EconomicEvents events ON trans.EventId = events.EventId    
                ) AS transctionDetails
                JOIN Shared.ExternalAgentTypes aTypes ON transctionDetails.AgentTypeId = aTypes.AgentTypeId
                WHERE transctionDetails.CashTransactionId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.CashTransactionId, DbType.Int32);

                using (var connection = ctx.CreateConnection())
                {
                    CashAccountTransactionDetail detail = await connection.QueryFirstOrDefaultAsync<CashAccountTransactionDetail>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a cash account transaction with Id '{queryParameters.CashTransactionId}'!";
                        return OperationResult<CashAccountTransactionDetail>.CreateFailure(msg);
                    }

                    return OperationResult<CashAccountTransactionDetail>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CashAccountTransactionDetail>.CreateFailure(ex.Message);
            }
        }
    }
}