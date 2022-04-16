using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing
{
    public class GetReceiptLoanProceedsValidationModelQuery
    {
        public async static Task<OperationResult<ReceiptLoanProceedsValidationModel>> Query(ReceiptLoanProceedsValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID, agree.LoanId, fin.FinancierName, agree.LoanAmount,
                    cash.CashAcctTransactionDate AS DateReceived, cash.CashAcctTransactionAmount AS AmountReceived 
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.LoanAgreements agree ON fin.FinancierID = agree.FinancierId
                LEFT JOIN Finance.CashAccountTransactions cash ON agree.LoanId = cash.EventID
                WHERE fin.FinancierID = @ID AND agree.LoanId = @LoanID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LoanID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    ReceiptLoanProceedsValidationModel model =
                        await connection.QueryFirstOrDefaultAsync<ReceiptLoanProceedsValidationModel>(sql, parameters);

                    if (model is null)
                    {
                        string msg = $"Unable to locate a loan agreement with loan Id: {queryParameters.LoanId} for financier: {queryParameters.FinancierId}.";
                        return OperationResult<ReceiptLoanProceedsValidationModel>.CreateFailure(msg);
                    }

                    return OperationResult<ReceiptLoanProceedsValidationModel>.CreateSuccessResult(model);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<ReceiptLoanProceedsValidationModel>.CreateFailure(ex.Message);
            }
        }
    }
}