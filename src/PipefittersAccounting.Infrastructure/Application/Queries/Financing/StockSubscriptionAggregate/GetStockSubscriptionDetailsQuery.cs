using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class GetStockSubscriptionReadModelQuery
    {
        public async static Task<OperationResult<StockSubscriptionReadModel>> Query(GetStockSubscriptionParameter queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    subs.StockId, subs.StockNumber, subs.FinancierId, fin.FinancierName AS InvestorName,
                    fin.AddressLine1 + ' ' + ISNULL(fin.AddressLine2, '') AS StreetAddress,
                    fin.City + ', ' + fin.StateCode + ' ' + fin.Zipcode AS CityStateZipcode,
                    fin.ContactFirstName + ' ' + ISNULL(fin.ContactMiddleInitial, '') + ' ' + fin.ContactLastName AS ContactName,
                    fin.ContactTelephone,  
                    subs.StockIssueDate, subs.SharesIssured, subs.PricePerShare,
                    subs.UserId, subs.CreatedDate, subs.LastModifiedDate
                FROM Finance.StockSubscriptions subs
                JOIN Finance.Financiers fin ON subs.FinancierId = fin.FinancierID
                WHERE subs.StockId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.StockId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    StockSubscriptionReadModel detail = await connection.QueryFirstOrDefaultAsync<StockSubscriptionReadModel>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a stock subscription with StockId '{queryParameters.StockId}'!";
                        return OperationResult<StockSubscriptionReadModel>.CreateFailure(msg);
                    }

                    return OperationResult<StockSubscriptionReadModel>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<StockSubscriptionReadModel>.CreateFailure(ex.Message);
            }
        }

        private async static Task<OperationResult<List<DividendDeclarationReadModel>>> Query(Guid stockId, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    dividend.DividendId,
                    sub.StockId,                     
                    sub.SharesIssured,                          
                    dividend.DividendDeclarationDate,
                    dividend.DividendPerShare,    
                    cash.CashAcctTransactionDate AS DatePaid, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid,
                    dividend.CreatedDate,
                    dividend.LastModifiedDate 
                FROM Finance.StockSubscriptions sub
                LEFT JOIN Finance.DividendDeclarations dividend ON sub.StockId = dividend.StockId
                LEFT JOIN CashManagement.CashTransactions cash ON dividend.DividendId = cash.EventID
                WHERE sub.StockId = @StockId";

                var parameters = new DynamicParameters();
                parameters.Add("StockId", stockId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<DividendDeclarationReadModel>(sql, parameters);

                    return OperationResult<List<DividendDeclarationReadModel>>.CreateSuccessResult(items.ToList());
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<DividendDeclarationReadModel>>.CreateFailure(ex.Message);
            }
        }
    }
}
