using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public class GetStockSubscriptionDetailsQuery
    {
        public async static Task<OperationResult<StockSubscriptionDetails>> Query(GetStockSubscriptionParameters queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    subs.StockId, subs.FinancierId, fin.FinancierName AS InvestorName,
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
                    StockSubscriptionDetails detail = await connection.QueryFirstOrDefaultAsync<StockSubscriptionDetails>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a stock subscription with StockId '{queryParameters.StockId}'!";
                        return OperationResult<StockSubscriptionDetails>.CreateFailure(msg);
                    }

                    return OperationResult<StockSubscriptionDetails>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<StockSubscriptionDetails>.CreateFailure(ex.Message);
            }
        }
    }
}