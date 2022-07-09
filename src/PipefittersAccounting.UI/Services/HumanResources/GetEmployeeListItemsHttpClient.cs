using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetEmployeeListItemsHttpClient
    {
        public static async Task<OperationResult<PagingResponse<EmployeeListItem>>> Query
        (
            GetEmployeesParameters queryParameters,
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

                var uri = $"1.0/employees/list";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<EmployeeListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<EmployeeListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    Console.WriteLine($"MetaData.CurrentPage: {pagingResponse.MetaData!.CurrentPage}, MetaData.PageSize: {pagingResponse.MetaData!.PageSize}");
                    return OperationResult<PagingResponse<EmployeeListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<EmployeeListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<EmployeeListItem>>.CreateFailure(ex.Message);
            }
        }

        public static async Task<OperationResult<PagingResponse<EmployeeListItem>>> Query
        (
            GetEmployeesByLastNameParameters queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["LastName"] = queryParameters!.LastName,
                    ["Page"] = queryParameters.Page.ToString(),
                    ["PageSize"] = queryParameters.PageSize.ToString()
                };

                var uri = $"1.0/employees/list";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<EmployeeListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<EmployeeListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    Console.WriteLine($"MetaData.CurrentPage: {pagingResponse.MetaData!.CurrentPage}, MetaData.PageSize: {pagingResponse.MetaData!.PageSize}");
                    return OperationResult<PagingResponse<EmployeeListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<EmployeeListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<EmployeeListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}