using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetManagersHttpClient
    {
        public static async Task<OperationResult<List<EmployeeManager>>> Query
        (
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = "1.0/employees/managers";
                using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                    var employeeManagers = await JsonSerializer.DeserializeAsync<List<EmployeeManager>>(jsonResponse, options);
                    return OperationResult<List<EmployeeManager>>.CreateSuccessResult(employeeManagers!);
                }
                else
                {
                    return OperationResult<List<EmployeeManager>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<EmployeeManager>>.CreateFailure(ex.Message);
            }
        }
    }
}