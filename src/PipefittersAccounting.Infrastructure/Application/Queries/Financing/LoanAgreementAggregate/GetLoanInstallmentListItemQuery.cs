using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class GetLoanInstallmentListItemQuery
    {
        public async static Task<OperationResult<List<LoanInstallmentListItem>>> Query(GetLoanAgreementInstallments queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    LoanInstallmentId, InstallmentNumber, PaymentDueDate, EqualMonthlyInstallment, 
                    PrincipalAmount, InterestAmount, PrincipalRemaining, 
                    cash.CashAcctTransactionDate AS DatePaid, cash.CashAcctTransactionAmount AS AmountPaid
                FROM Finance.LoanInstallments installments
                LEFT JOIN Finance.CashAccountTransactions cash ON installments.LoanInstallmentId = cash.EventID
                WHERE LoanId = @ID
                ORDER BY InstallmentNumber";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    var items = await connection.QueryAsync<LoanInstallmentListItem>(sql, parameters);
                    return OperationResult<List<LoanInstallmentListItem>>.CreateSuccessResult(items.ToList());
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<LoanInstallmentListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}