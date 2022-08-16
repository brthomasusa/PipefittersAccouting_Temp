using System.Data;
using Dapper;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate
{
    public class GetLoanAgreementDetailsQuery
    {
        public async static Task<OperationResult<LoanAgreementDetail>> Query(GetLoanAgreement queryParameters, DapperContext ctx)
        {
            try
            {
                var sql =
                @"SELECT 
                    LoanId, LoanNumber, agreements.FinancierId, LoanAmount, InterestRate, LoanDate, MaturityDate,  
                    NumberOfInstallments, creditors.FinancierName, creditors.AddressLine1, creditors.AddressLine2,
                    creditors.City + ', ' + creditors.StateCode + ' ' + creditors.Zipcode AS CityStateZipcode, creditors.Telephone,
                    creditors.ContactFirstName + ' ' + ISNULL(creditors.ContactMiddleInitial, '') + ' ' + creditors.ContactLastName + ' (' + 
                    creditors.ContactTelephone + ') '  AS PointOfContact, agreements.UserId 
                FROM Finance.LoanAgreements agreements
                INNER JOIN Finance.Financiers creditors ON agreements.FinancierId = creditors.FinancierID       
                WHERE LoanId = @ID";

                var parameters = new DynamicParameters();
                parameters.Add("ID", queryParameters.LoanId, DbType.Guid);

                using (var connection = ctx.CreateConnection())
                {
                    LoanAgreementDetail detail = await connection.QueryFirstOrDefaultAsync<LoanAgreementDetail>(sql, parameters);
                    if (detail is null)
                    {
                        string msg = $"Unable to locate a loan agreement with LoanId: {queryParameters.LoanId}.";
                        return OperationResult<LoanAgreementDetail>.CreateFailure(msg);
                    }

                    OperationResult<List<LoanInstallmentDetail>> getInstallmentsResult = await GetLoanInstallments(queryParameters, ctx);

                    if (getInstallmentsResult.Success)
                    {
                        detail.LoanInstallmentDetailsList = getInstallmentsResult.Result;
                        return OperationResult<LoanAgreementDetail>.CreateSuccessResult(detail);
                    }
                    else
                    {
                        return OperationResult<LoanAgreementDetail>.CreateFailure(getInstallmentsResult.NonSuccessMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult<LoanAgreementDetail>.CreateFailure(ex.Message);
            }
        }

        private async static Task<OperationResult<List<LoanInstallmentDetail>>> GetLoanInstallments(GetLoanAgreement queryParameters, DapperContext ctx)
        {
            GetLoanAgreementInstallments installmentParams = new() { LoanId = queryParameters.LoanId };

            OperationResult<List<LoanInstallmentDetail>> getInstallmentsResult =
                await GetLoanInstallmentDetailQuery.Query(installmentParams, ctx);

            if (getInstallmentsResult.Success)
            {
                return getInstallmentsResult;
            }
            else
            {
                return OperationResult<List<LoanInstallmentDetail>>.CreateFailure(getInstallmentsResult.NonSuccessMessage);
            }
        }
    }
}