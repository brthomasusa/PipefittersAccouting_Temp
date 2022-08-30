using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetDividendDeclarationsHttpClient
    {
        public static async Task<OperationResult<PagingResponse<DividendDeclarationListItem>>> Query
        (
            GetDividendDeclarationsParameters queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["stockId"] = queryParameters.StockId.ToString(),
                    ["Page"] = queryParameters.Page.ToString(),
                    ["PageSize"] = queryParameters.PageSize.ToString()
                };

                var uri = $"1.0/stocksubscriptions/dividenddeclarations";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<DividendDeclarationListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<DividendDeclarationListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<DividendDeclarationListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<DividendDeclarationListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<DividendDeclarationListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}