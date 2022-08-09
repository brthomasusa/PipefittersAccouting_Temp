using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class VerifyCashDepositForDebtIssueProceedsQuery
    {
        public async static Task<OperationResult<decimal>> Query(ReceiptLoanProceedsValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountReceived
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.LoanAgreements agree ON fin.FinancierID = agree.FinancierId
                LEFT JOIN CashManagement.CashTransactions cash ON agree.LoanId = cash.EventID
                WHERE fin.FinancierID = @FINANCIERID AND agree.LoanId = @LOANID";

                var parameters = new DynamicParameters();
                parameters.Add("FINANCIERID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LOANID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    decimal amountRcvd = await connection.ExecuteScalarAsync<decimal>(sql, parameters);
                    return OperationResult<decimal>.CreateSuccessResult(amountRcvd);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<decimal>.CreateFailure(ex.Message);
            }
        }
    }
}