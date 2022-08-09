using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Queries.CashManagement
{
    public class GetReceiptLoanProceedsValidationModelQuery
    {
        public async static Task<OperationResult<DepositLoanProceedsValidationModel>> Query(ReceiptLoanProceedsValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID, agree.LoanId, fin.FinancierName, agree.LoanAmount,
                    cash.CashAcctTransactionDate AS DateReceived, cash.CashAcctTransactionAmount AS AmountReceived 
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.LoanAgreements agree ON fin.FinancierID = agree.FinancierId
                LEFT JOIN CashManagement.CashTransactions cash ON agree.LoanId = cash.EventID
                WHERE fin.FinancierID = @ID AND agree.LoanId = @LoanID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LoanID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    DepositLoanProceedsValidationModel model =
                        await connection.QueryFirstOrDefaultAsync<DepositLoanProceedsValidationModel>(sql, parameters);

                    if (model is null)
                    {
                        string msg = $"Unable to locate a loan agreement with loan Id: {queryParameters.LoanId} for financier: {queryParameters.FinancierId}.";
                        return OperationResult<DepositLoanProceedsValidationModel>.CreateFailure(msg);
                    }

                    return OperationResult<DepositLoanProceedsValidationModel>.CreateSuccessResult(model);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<DepositLoanProceedsValidationModel>.CreateFailure(ex.Message);
            }
        }
    }
}