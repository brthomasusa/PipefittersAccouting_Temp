using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetTimeCardsForManagerHttpClient
    {
        public static async Task<OperationResult<List<TimeCardWithPymtInfo>>> Query
        (
            GetTimeCardsForManagerParameter queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["supervisorId"] = queryParameters.SupervisorId.ToString(),
                    ["payPeriodEndDate"] = queryParameters.PayPeriodEndDate.ToShortDateString()
                };

                var uri = $"1.0/employees/timecards/timecardsformanager";

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