using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetFinanciersHttpClient
    {
        public static async Task<OperationResult<PagingResponse<FinancierListItems>>> Query
        (
            GetFinanciers queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["Page"] = queryParameters.Page.ToString(),
                    ["PageSize"] = queryParameters.PageSize.ToString()
                };

                var uri = $"1.0/financiers/list";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<FinancierListItems>
                    {
                        Items = JsonSerializer.Deserialize<List<FinancierListItems>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<FinancierListItems>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<FinancierListItems>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<FinancierListItems>>.CreateFailure(ex.Message);
            }
        }

        public static async Task<OperationResult<PagingResponse<FinancierListItems>>> Query
        (
            GetFinanciersByName queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["Name"] = queryParameters.Name,
                    ["Page"] = queryParameters.Page.ToString(),
                    ["PageSize"] = queryParameters.PageSize.ToString()
                };

                var uri = $"1.0/financiers/search";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<FinancierListItems>
                    {
                        Items = JsonSerializer.Deserialize<List<FinancierListItems>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<FinancierListItems>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<FinancierListItems>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<FinancierListItems>>.CreateFailure(ex.Message);
            }
        }
    }
}