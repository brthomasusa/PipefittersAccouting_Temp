using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetTimeCardsForPayPeriodHttpClient
    {
        public static async Task<OperationResult<List<TimeCardWithPymtInfo>>> Query
        (
            GetTimeCardsForPayPeriodParameter queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["payPeriodEndDate"] = queryParameters.PayPeriodEndDate.ToShortDateString(),
                    ["userId"] = queryParameters.UserId.ToString()
                };

                var uri = $"1.0/employees/timecards/timecardsforpayperiod";

                List<TimeCardWithPymtInfo>? response =
                    await client.GetFromJsonAsync<List<TimeCardWithPymtInfo>>(QueryHelpers.AddQueryString(uri, queryParams));

                return OperationResult<List<TimeCardWithPymtInfo>>.CreateSuccessResult(response!);
            }
            catch (Exception ex)
            {
                return OperationResult<List<TimeCardWithPymtInfo>>.CreateFailure(ex.Message);
            }
        }
    }
}