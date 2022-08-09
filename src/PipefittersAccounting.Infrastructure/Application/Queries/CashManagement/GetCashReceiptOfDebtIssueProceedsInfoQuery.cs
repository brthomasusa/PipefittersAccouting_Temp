using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Queries.CashManagement
{
    public class GetCashReceiptOfDebtIssueProceedsInfoQuery
    {
        public async static Task<OperationResult<CashReceiptOfDebtIssueProceedsInfo>> Query(CreditorLoanAgreementValidationParameters queryParameters,
                                                                                             DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID, agree.LoanId, fin.FinancierName, agree.LoanDate, 
                    agree.MaturityDate, agree.LoanAmount,
                    cash.CashAcctTransactionDate AS DateReceived, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountReceived
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.LoanAgreements agree ON fin.FinancierID = agree.FinancierId
                LEFT JOIN CashManagement.CashTransactions cash ON agree.LoanId = cash.EventID
                WHERE  fin.FinancierID = @FINANCIERID AND agree.LoanId = @LOANID";

                var parameters = new DynamicParameters();
                parameters.Add("FINANCIERID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LOANID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    CashReceiptOfDebtIssueProceedsInfo loanInfo =
                        await connection.QueryFirstOrDefaultAsync<CashReceiptOfDebtIssueProceedsInfo>(sql, parameters);

                    if (loanInfo is null)
                    {
                        string msg = $"Unable to locate a loan agreement with id '{queryParameters.LoanId}' issued by financier with id '{queryParameters.FinancierId}'!";
                        return OperationResult<CashReceiptOfDebtIssueProceedsInfo>.CreateFailure(msg);
                    }

                    return OperationResult<CashReceiptOfDebtIssueProceedsInfo>.CreateSuccessResult(loanInfo);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<CashReceiptOfDebtIssueProceedsInfo>.CreateFailure(ex.Message);
            }
        }
    }
}