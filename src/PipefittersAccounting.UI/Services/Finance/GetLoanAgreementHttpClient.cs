using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetLoanAgreementHttpClient
    {
        public static async Task<OperationResult<LoanAgreementReadModel>> Query
        (
            GetLoanAgreement queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = $"1.0/loanagreements/detail/{queryParameters.LoanId}";
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LoanAgreementReadModel? loan = JsonSerializer.Deserialize<LoanAgreementReadModel>(content, options);
                    if (loan is not null)
                    {
                        return OperationResult<LoanAgreementReadModel>.CreateSuccessResult(loan);
                    }
                    else
                    {
                        return OperationResult<LoanAgreementReadModel>.CreateFailure($"A loan agreement with Id: '{queryParameters.LoanId}' was not found.");
                    }
                }
                else
                {
                    return OperationResult<LoanAgreementReadModel>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<LoanAgreementReadModel>.CreateFailure(ex.Message);
            }
        }
    }
}