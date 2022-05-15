using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.CashAccountAggregate
{
    public class GetDisburesementLoanPymtValidationModelQuery
    {
        public async static Task<OperationResult<DisburesementLoanPymtValidationModel>> Query(DisburesementLoanPymtValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    agree.FinancierId, agree.LoanId, LoanInstallmentId, 
                    InstallmentNumber, PaymentDueDate, EqualMonthlyInstallment,  
                    cash.CashAcctTransactionDate AS DatePaid, cash.CashAcctTransactionAmount AS AmountPaid
                FROM Finance.LoanAgreements agree
                LEFT JOIN Finance.LoanInstallments installments ON agree.LoanId = installments.LoanId
                LEFT JOIN Finance.CashAccountTransactions cash ON installments.LoanInstallmentId = cash.EventID
                WHERE installments.LoanInstallmentId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.LoanInstallmentId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    DisburesementLoanPymtValidationModel model =
                        await connection.QueryFirstOrDefaultAsync<DisburesementLoanPymtValidationModel>(sql, parameters);

                    if (model is null)
                    {
                        string msg = $"Unable to locate a loan installment with Id: {queryParameters.LoanInstallmentId}.";
                        return OperationResult<DisburesementLoanPymtValidationModel>.CreateFailure(msg);
                    }

                    return OperationResult<DisburesementLoanPymtValidationModel>.CreateSuccessResult(model);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<DisburesementLoanPymtValidationModel>.CreateFailure(ex.Message);
            }
        }
    }
}