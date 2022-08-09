using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class VerifyCreditorIsLinkedToLoanAgreementQuery
    {
        public async static Task<OperationResult<Guid>> Query(ReceiptLoanProceedsValidationParams queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    fin.FinancierID
                FROM Finance.Financiers fin 
                LEFT JOIN Finance.LoanAgreements agree ON fin.FinancierID = agree.FinancierId
                WHERE fin.FinancierID = @FINANCIERID AND agree.LoanId = @LOANID";

                var parameters = new DynamicParameters();
                parameters.Add("FINANCIERID", queryParameters.FinancierId, DbType.Guid);
                parameters.Add("LOANID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    Guid creditorID = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
                    return OperationResult<Guid>.CreateSuccessResult(creditorID);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.CreateFailure(ex.Message);
            }
        }
    }
}