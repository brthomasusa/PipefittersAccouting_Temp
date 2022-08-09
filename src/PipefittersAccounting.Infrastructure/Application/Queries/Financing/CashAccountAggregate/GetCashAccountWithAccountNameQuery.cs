using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetCashAccountWithAccountNameQuery
    {
        public async static Task<OperationResult<CashAccountReadModel>> Query(GetCashAccountWithAccountName queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    CashAccountId, CashAccountTypeId, BankName, AccountName, AccountNumber,
                    RoutingTransitNumber, DateOpened, UserId, CreatedDate, LastModifiedDate
                FROM CashManagement.CashAccounts
                WHERE AccountName = @ACCTNAME";

                var parameters = new DynamicParameters();
                parameters.Add("ACCTNAME", queryParameters.AccountName, DbType.String);

                using (var connection = ctx.CreateConnection())
                {
                    CashAccountReadModel detail = await connection.QueryFirstOrDefaultAsync<CashAccountReadModel>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a cash account with account name '{queryParameters.AccountName}'!";
                        return OperationResult<CashAccountReadModel>.CreateFailure(msg);
                    }

                    return OperationResult<CashAccountReadModel>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CashAccountReadModel>.CreateFailure(ex.Message);
            }
        }
    }
}