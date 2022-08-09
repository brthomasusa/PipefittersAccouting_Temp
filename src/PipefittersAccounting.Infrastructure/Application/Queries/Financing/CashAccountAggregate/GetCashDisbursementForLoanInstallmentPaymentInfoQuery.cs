using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetCashDisbursementForLoanInstallmentPaymentInfoQuery
    {
        public async static Task<OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>> Query(GetLoanInstallmentInfoParameters queryParameters,
                                                                                                       DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    agree.FinancierId, agree.LoanId, LoanInstallmentId,        
                    agree.LoanDate, agree.MaturityDate, EqualMonthlyInstallment,           
                    cash.CashAcctTransactionDate AS DatePaid,
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid     
                FROM Finance.LoanAgreements agree
                LEFT JOIN Finance.LoanInstallments installments ON agree.LoanId = installments.LoanId
                LEFT JOIN CashManagement.CashTransactions cash ON installments.LoanInstallmentId = cash.EventID
                WHERE  installments.LoanInstallmentId = @INSTALLMENTID";

                var parameters = new DynamicParameters();
                parameters.Add("INSTALLMENTID", queryParameters.LoanInstallmentId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    CashDisbursementForLoanInstallmentPaymentInfo installmentInfo =
                        await connection.QueryFirstOrDefaultAsync<CashDisbursementForLoanInstallmentPaymentInfo>(sql, parameters);

                    if (installmentInfo is null)
                    {
                        string msg = $"Unable to locate a loan installment with id '{queryParameters.LoanInstallmentId}'!";
                        return OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>.CreateFailure(msg);
                    }

                    return OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>.CreateSuccessResult(installmentInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CashDisbursementForLoanInstallmentPaymentInfo>.CreateFailure(ex.Message);
            }
        }
    }
}
