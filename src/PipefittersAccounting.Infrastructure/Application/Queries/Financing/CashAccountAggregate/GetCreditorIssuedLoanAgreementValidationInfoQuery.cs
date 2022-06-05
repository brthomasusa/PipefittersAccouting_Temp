using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetCreditorIssuedLoanAgreementValidationInfoQuery
    {
        public async static Task<OperationResult<CreditorIssuedLoanAgreementValidationInfo>> Query(CreditorLoanAgreementValidationParameters queryParameters,
                                                                                             DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID, fin.FinancierName, agree.LoanId, agree.LoanAmount
                FROM Finance.LoanAgreements agree
                LEFT JOIN Finance.Financiers fin ON agree.FinancierId = fin.FinancierID
                WHERE  agree.LoanId = @LOANID AND agree.FinancierId = @FINANCIERID";

                var parameters = new DynamicParameters();
                parameters.Add("LOANID", queryParameters.LoanId, DbType.Guid);
                parameters.Add("FINANCIERID", queryParameters.FinancierId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    CreditorIssuedLoanAgreementValidationInfo loanInfo =
                        await connection.QueryFirstOrDefaultAsync<CreditorIssuedLoanAgreementValidationInfo>(sql, parameters);

                    if (loanInfo is null)
                    {
                        string msg = $"Unable to locate a loan agreement with id '{queryParameters.LoanId}' issued by financier with id '{queryParameters.FinancierId}'!";
                        return OperationResult<CreditorIssuedLoanAgreementValidationInfo>.CreateFailure(msg);
                    }

                    return OperationResult<CreditorIssuedLoanAgreementValidationInfo>.CreateSuccessResult(loanInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CreditorIssuedLoanAgreementValidationInfo>.CreateFailure(ex.Message);
            }
        }
    }
}