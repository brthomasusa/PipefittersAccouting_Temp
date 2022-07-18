using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.FinancierAggregate
{
    public class GetFinancierDetailsQuery
    {
        public async static Task<OperationResult<FinancierReadModel>> Query(GetFinancier queryParameters, DapperContext ctx)
        {
            try
            {
                //TODO Remove check for financier id, handle null financier with OperationResult.FailureMessage
                if (await IsValidFinancierID(queryParameters.FinancierId, ctx) == false)
                {
                    string errMsg = $"No financier record found where FinancierId equals {queryParameters.FinancierId}.";
                    return OperationResult<FinancierReadModel>.CreateFailure(errMsg);
                }

                var sql =
                @"SELECT 
                    FinancierID, FinancierName, Telephone, 
                    AddressLine1, AddressLine2, City, StateCode, Zipcode,
                    AddressLine1 + ' ' + ISNULL(AddressLine2, '') + ' ' + City + ', ' + StateCode + ' ' + Zipcode AS FullAddress, 
                    ContactFirstName, ContactLastName, ContactMiddleInitial, 
                    ContactFirstName + ' ' + ISNULL(ContactMiddleInitial, '') + ' ' + ContactLastName AS 'ContactFullName',
                    ContactTelephone,
                    IsActive, UserId, CreatedDate, LastModifiedDate
                FROM Finance.Financiers       
                WHERE FinancierId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.FinancierId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    FinancierReadModel detail = await connection.QueryFirstOrDefaultAsync<FinancierReadModel>(sql, parameters);
                    return OperationResult<FinancierReadModel>.CreateSuccessResult(detail);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<FinancierReadModel>.CreateFailure(ex.Message);
            }

        }

        private async static Task<bool> IsValidFinancierID(Guid financierId, DapperContext ctx)
        {
            string sql = $"SELECT FinancierID FROM Finance.Financiers WHERE FinancierId = @ID";
            var parameters = new DynamicParameters();
            parameters.Add("ID", financierId, DbType.Guid);

            using (var connection = ctx.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync(sql, parameters);
                return result != null;
            }
        }
    }
}