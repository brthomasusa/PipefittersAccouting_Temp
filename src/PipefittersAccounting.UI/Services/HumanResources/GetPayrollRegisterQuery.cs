using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetPayrollRegisterQuery
    {
        public static async Task<OperationResult<List<PayrollRegister>>> Query
        (
            GetPayrollRegisterParameter queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["PayPeriodEnded"] = queryParameters.PayPeriodEnded.ToShortDateString()
                };

                var uri = $"1.0/employees/timecards/payrollregister";

                List<PayrollRegister>? response =
                    await client.GetFromJsonAsync<List<PayrollRegister>>(QueryHelpers.AddQueryString(uri, queryParams));

                return OperationResult<List<PayrollRegister>>.CreateSuccessResult(response!);
            }
            catch (Exception ex)
            {
                return OperationResult<List<PayrollRegister>>.CreateFailure(ex.Message);
            }
        }
    }
}