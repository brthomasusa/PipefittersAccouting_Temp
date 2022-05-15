using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetCreditorHasLoanAgreeValidationModelQuery
    {
        public async static Task<OperationResult<CreditorHasLoanAgreeValidationModel>> Query(CreditorHasLoanAgreeValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierName, agree.LoanAmount
                FROM Finance.LoanAgreements agree
                LEFT JOIN Finance.Financiers fin ON agree.FinancierId = fin.FinancierID               
                WHERE  agree.LoanId = @LoanID AND agree.FinancierId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LoanID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    CreditorHasLoanAgreeValidationModel model =
                        await connection.QueryFirstOrDefaultAsync<CreditorHasLoanAgreeValidationModel>(sql, parameters);

                    if (model is null)
                    {
                        string msg = $"Unable to locate a loan agreement with loan Id: {queryParameters.LoanId} for financier: {queryParameters.FinancierId}.";
                        return OperationResult<CreditorHasLoanAgreeValidationModel>.CreateFailure(msg);
                    }

                    return OperationResult<CreditorHasLoanAgreeValidationModel>.CreateSuccessResult(model);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CreditorHasLoanAgreeValidationModel>.CreateFailure(ex.Message);
            }
        }
    }
}