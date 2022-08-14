using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class GetLoanAgreementsHttpClient
    {
        public static async Task<OperationResult<PagingResponse<LoanAgreementListItem>>> Query
        (
            GetLoanAgreements queryParameters,
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

                var uri = $"1.0/loanagreements/list";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<LoanAgreementListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<LoanAgreementListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<LoanAgreementListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<LoanAgreementListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<LoanAgreementListItem>>.CreateFailure(ex.Message);
            }
        }

        public static async Task<OperationResult<PagingResponse<LoanAgreementListItem>>> Query
        (
            GetLoanAgreementByLoanNumber queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["loanNumber"] = queryParameters.LoanNumber,
                    ["page"] = queryParameters.Page.ToString(),
                    ["pageSize"] = queryParameters.PageSize.ToString()
                };

                var uri = $"1.0/loanagreements/search";
                var response = await client.GetAsync(QueryHelpers.AddQueryString(uri, queryParams));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagingResponse = new PagingResponse<LoanAgreementListItem>
                    {
                        Items = JsonSerializer.Deserialize<List<LoanAgreementListItem>>(content, options),
                        MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
                    };
                    return OperationResult<PagingResponse<LoanAgreementListItem>>.CreateSuccessResult(pagingResponse);
                }
                else
                {
                    return OperationResult<PagingResponse<LoanAgreementListItem>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<PagingResponse<LoanAgreementListItem>>.CreateFailure(ex.Message);
            }
        }
    }
}