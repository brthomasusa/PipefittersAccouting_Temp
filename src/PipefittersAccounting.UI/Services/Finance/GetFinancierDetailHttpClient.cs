using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetFinancierDetailHttpClient
    {
        public static async Task<OperationResult<FinancierDetail>> Query
        (
            GetFinancier queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = $"1.0/financiers/detail/{queryParameters.FinancierId}";
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    FinancierDetail? financierDetail = JsonSerializer.Deserialize<FinancierDetail>(content, options);
                    if (financierDetail is not null)
                    {
                        return OperationResult<FinancierDetail>.CreateSuccessResult(financierDetail);
                    }
                    else
                    {
                        return OperationResult<FinancierDetail>.CreateFailure($"A financier with Id: '{queryParameters.FinancierId}' was not found.");
                    }
                }
                else
                {
                    return OperationResult<FinancierDetail>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<FinancierDetail>.CreateFailure(ex.Message);
            }
        }
    }
}