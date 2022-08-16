using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class GetLoanInstallmentDetailQuery
    {
        public async static Task<OperationResult<List<LoanInstallmentDetail>>> Query(GetLoanAgreementInstallments queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    LoanInstallmentId, LoanId, InstallmentNumber, PaymentDueDate, EqualMonthlyInstallment, 
                    PrincipalAmount, InterestAmount, PrincipalRemaining,                    
                    cash.CashAcctTransactionDate AS DatePaid, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountPaid,
                    installments.UserId
                FROM Finance.LoanInstallments installments
                LEFT JOIN CashManagement.CashTransactions cash ON installments.LoanInstallmentId = cash.EventID
                WHERE LoanId = @ID
                ORDER BY InstallmentNumber";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<LoanInstallmentDetail>(sql, parameters);
                    return OperationResult<List<LoanInstallmentDetail>>.CreateSuccessResult(items.ToList());
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<LoanInstallmentDetail>>.CreateFailure(ex.Message);
            }
        }
    }
}