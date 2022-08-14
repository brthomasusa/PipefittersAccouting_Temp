using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetFinanciersLookupHttpClient
    {
        public static async Task<OperationResult<List<FinancierLookup>>> Query
        (
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = "1.0/financiers/financierslookup";
                using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                    var lookups = await JsonSerializer.DeserializeAsync<List<FinancierLookup>>(jsonResponse, options);
                    return OperationResult<List<FinancierLookup>>.CreateSuccessResult(lookups!);
                }
                else
                {
                    return OperationResult<List<FinancierLookup>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<FinancierLookup>>.CreateFailure(ex.Message);
            }
        }
    }
}