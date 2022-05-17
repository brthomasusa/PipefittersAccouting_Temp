using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetFinancierToLoanInstallmentValidationInfoQuery
    {
        public async static Task<OperationResult<FinancierToLoanInstallmentValidationInfo>> Query(GetLoanInstallmentInfoParameters queryParameters,
                                                                                                       DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    agree.FinancierId, agree.LoanId, install.LoanInstallmentId
                FROM Finance.LoanAgreements agree
                LEFT JOIN Finance.LoanInstallments install ON agree.LoanId = install.LoanId
                WHERE  install.LoanInstallmentId = @INSTALLMENTID";

                var parameters = new DynamicParameters();
                parameters.Add("INSTALLMENTID", queryParameters.LoanInstallmentId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    FinancierToLoanInstallmentValidationInfo installmentInfo =
                        await connection.QueryFirstOrDefaultAsync<FinancierToLoanInstallmentValidationInfo>(sql, parameters);

                    if (installmentInfo is null)
                    {
                        string msg = $"Unable to locate a loan installment with id '{queryParameters.LoanInstallmentId}'!";
                        return OperationResult<FinancierToLoanInstallmentValidationInfo>.CreateFailure(msg);
                    }

                    return OperationResult<FinancierToLoanInstallmentValidationInfo>.CreateSuccessResult(installmentInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<FinancierToLoanInstallmentValidationInfo>.CreateFailure(ex.Message);
            }
        }
    }
}