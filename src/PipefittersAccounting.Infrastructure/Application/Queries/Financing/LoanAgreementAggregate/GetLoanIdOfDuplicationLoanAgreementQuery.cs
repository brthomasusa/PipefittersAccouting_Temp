using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class GetLoanIdOfDuplicationLoanAgreementQuery
    {
        public async static Task<OperationResult<Guid>> Query(GetDuplicateLoanAgreement queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    LoanId
                FROM Finance.LoanAgreements
                WHERE FinancierId = @FINANCIERID 
                    AND LoanAmount = @LOANAMT 
                    AND InterestRate = @INTERESTRATE
                    AND LoanDate = @LOANDATE 
                    AND MaturityDate = @MATURITY";

                var parameters = new DynamicParameters();
                parameters.Add("FINANCIERID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LOANAMT", queryParameters.LoanAmount, DbType.Decimal);
                parameters.Add("INTERESTRATE", queryParameters.InterestRate, DbType.Decimal);
                parameters.Add("LOANDATE", queryParameters.LoanDate, DbType.DateTime2);
                parameters.Add("MATURITY", queryParameters.MaturityDate, DbType.DateTime2);

                using (var connection = ctx.CreateConnection())
                {
                    Guid loanID = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
                    return OperationResult<Guid>.CreateSuccessResult(loanID);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex.Message);
            }
        }
    }
}