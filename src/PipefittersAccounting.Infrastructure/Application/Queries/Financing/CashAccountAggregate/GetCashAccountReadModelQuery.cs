using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetCashAccountReadModelQuery
    {
        public async static Task<OperationResult<CashAccountReadModel>> Query(GetCashAccount queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    CashAccountId, CashAccountTypeId, BankName, AccountName, AccountNumber,
                    RoutingTransitNumber, DateOpened, UserId, CreatedDate, LastModifiedDate
                FROM Finance.CashAccounts
                WHERE CashAccountId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.CashAccountId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    CashAccountReadModel detail = await connection.QueryFirstOrDefaultAsync<CashAccountReadModel>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a cash account with Id '{queryParameters.CashAccountId}'!";
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