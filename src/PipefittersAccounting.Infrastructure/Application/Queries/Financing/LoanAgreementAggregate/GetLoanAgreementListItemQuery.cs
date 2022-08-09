using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class GetLoanAgreementListItemQuery
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public async static Task<OperationResult<PagedList<LoanAgreementListItem>>> Query(GetLoanAgreements queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    LoanId, LoanNumber, creditors.FinancierName, LoanAmount, InterestRate, LoanDate, MaturityDate, 
                    NumberOfInstallments, cash.CashAcctTransactionDate AS LoanProceedsReceived , 
                    cash.CashAcctTransactionAmount LoanProceedsAmount    
                FROM Finance.LoanAgreements agreements
                INNER JOIN Finance.Financiers creditors ON agreements.FinancierId = creditors.FinancierID
                LEFT JOIN CashManagement.CashTransactions cash ON agreements.LoanId = cash.EventID
                ORDER BY creditors.FinancierName, LoanDate
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var parameters = new DynamicParameters();
                parameters.Add("Offset", Offset(queryParameters.Page, queryParameters.PageSize), DbType.Int32);
                parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);

                var totalRecordsSql = $"SELECT COUNT(LoanId) FROM Finance.LoanAgreements";

                using (var connection = ctx.CreateConnection())
                {
                    int count = await connection.ExecuteScalarAsync<int>(totalRecordsSql);
                    var items = await connection.QueryAsync<LoanAgreementListItem>(sql, parameters);
                    var pagedList = PagedList<LoanAgreementListItem>.CreatePagedList(items.ToList(), count, queryParameters.Page, queryParameters.PageSize);

                    return OperationResult<PagedList<LoanAgreementListItem>>.CreateSuccessResult(pagedList);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagedList<LoanAgreementListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}