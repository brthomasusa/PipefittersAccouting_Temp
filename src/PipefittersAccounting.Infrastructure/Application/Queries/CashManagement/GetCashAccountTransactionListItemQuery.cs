using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.Infrastructure.Application.Queries.CashManagement
{
    public class GetCashAccountTransactionListItemQuery
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<CashAccountTransactionListItem>>> Query(GetCashAccountTransactionListItemsParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    transctionDetails.CashTransactionId,  transctionDetails.CashAcctTransactionDate,
                    transctionDetails.CashAcctTransactionAmount, transctionDetails.AgentId,
                    aTypes.AgentTypeId, aTypes.AgentTypeName, transctionDetails.CashTransactionTypeName       
                FROM 
                (
                    SELECT 
                        trans.CashTransactionId, trans.CashAccountId, trans.CashTransactionTypeId, transTypes.CashTransactionTypeName,
                        trans.CashAcctTransactionDate, trans.CashAcctTransactionAmount, trans.AgentId, agents.AgentTypeId        
                    FROM CashManagement.CashTransactions trans
                    JOIN CashManagement.CashTransactionTypes transTypes ON trans.CashTransactionTypeId = transTypes.CashTransactionTypeId
                    JOIN Shared.ExternalAgents agents ON trans.AgentId = agents.AgentId
                    JOIN Shared.EconomicEvents events ON trans.EventId = events.EventId    
                ) AS transctionDetails
                JOIN Shared.ExternalAgentTypes aTypes ON transctionDetails.AgentTypeId = aTypes.AgentTypeId -- aTypes.AgentTypeName
                WHERE transctionDetails.CashAccountId = @ID
                ORDER BY transctionDetails.CashTransactionId
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.CashAccountId, DbType.Guid);
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var countParam = new { CASHACCOUNTID = queryParameters.CashAccountId };
                var totalRecordsSql = $"SELECT COUNT(CashTransactionId) FROM CashManagement.CashTransactions WHERE CashAccountId = @CASHACCOUNTID";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql, countParam);
                    var items = await connection.QueryAsync<CashAccountTransactionListItem>(sql, parameters);
                    var pagedList = PagedList<CashAccountTransactionListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<CashAccountTransactionListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<CashAccountTransactionListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}