using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetStockSubscriptionHttpClient
    {
        public static async Task<OperationResult<StockSubscriptionReadModel>> Query
        (
            GetStockSubscriptionParameter queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = $"1.0/stocksubscriptions/detail/{queryParameters.StockId}";
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    StockSubscriptionReadModel? loan = JsonSerializer.Deserialize<StockSubscriptionReadModel>(content, options);
                    if (loan is not null)
                    {
                        return OperationResult<StockSubscriptionReadModel>.CreateSuccessResult(loan);
                    }
                    else
                    {
                        return OperationResult<StockSubscriptionReadModel>.CreateFailure($"A stock subscription with Id: '{queryParameters.StockId}' was not found.");
                    }
                }
                else
                {
                    return OperationResult<StockSubscriptionReadModel>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<StockSubscriptionReadModel>.CreateFailure(ex.Message);
            }
        }
    }
}