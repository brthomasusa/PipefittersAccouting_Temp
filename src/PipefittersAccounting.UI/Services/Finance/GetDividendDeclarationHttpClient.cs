using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetDividendDeclarationHttpClient
    {
        public static async Task<OperationResult<DividendDeclarationReadModel>> Query
        (
            GetDividendDeclarationParameter queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = $"1.0/stocksubscriptions/dividenddeclaration/{queryParameters.DividendId}";
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    DividendDeclarationReadModel? detail = JsonSerializer.Deserialize<DividendDeclarationReadModel>(content, options);
                    if (detail is not null)
                    {
                        return OperationResult<DividendDeclarationReadModel>.CreateSuccessResult(detail);
                    }
                    else
                    {
                        return OperationResult<DividendDeclarationReadModel>.CreateFailure($"A financier with Id: '{queryParameters.DividendId}' was not found.");
                    }
                }
                else
                {
                    return OperationResult<DividendDeclarationReadModel>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<DividendDeclarationReadModel>.CreateFailure(ex.Message);
            }
        }
    }
}