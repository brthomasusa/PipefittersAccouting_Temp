using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetFinancierHttpClient
    {
        public static async Task<OperationResult<FinancierReadModel>> Query
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
                    FinancierReadModel? financierDetail = JsonSerializer.Deserialize<FinancierReadModel>(content, options);
                    if (financierDetail is not null)
                    {
                        return OperationResult<FinancierReadModel>.CreateSuccessResult(financierDetail);
                    }
                    else
                    {
                        return OperationResult<FinancierReadModel>.CreateFailure($"A financier with Id: '{queryParameters.FinancierId}' was not found.");
                    }
                }
                else
                {
                    return OperationResult<FinancierReadModel>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<FinancierReadModel>.CreateFailure(ex.Message);
            }
        }
    }
}