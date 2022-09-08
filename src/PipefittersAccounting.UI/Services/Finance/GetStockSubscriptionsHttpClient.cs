using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetStockSubscriptionsHttpClient
    {
        public static async Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> Query
        (
            GetStockSubscriptionListItem queryParameters,
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

                var uri = $"1.0/stocksubscriptions/list";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<StockSubscriptionListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<StockSubscriptionListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }

        public static async Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> Query
        (
            GetStockSubscriptionListItemByInvestorName queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["InvestorName"] = queryParameters.InvestorName,
                    ["Page"] = queryParameters.Page.ToString(),
                    ["PageSize"] = queryParameters.PageSize.ToString()
                };

                var uri = $"1.0/stocksubscriptions/search";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<StockSubscriptionListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<StockSubscriptionListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }

        public static async Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> QueryByFundsRcvd
        (
            GetStockSubscriptionListItem queryParameters,
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

                var uri = $"1.0/stocksubscriptions/fundsrcvd";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<StockSubscriptionListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<StockSubscriptionListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }

        public static async Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> QueryByFundsNotRcvd
        (
            GetStockSubscriptionListItem queryParameters,
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

                var uri = $"1.0/stocksubscriptions/fundsnotrcvd";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<StockSubscriptionListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<StockSubscriptionListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<StockSubscriptionListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}