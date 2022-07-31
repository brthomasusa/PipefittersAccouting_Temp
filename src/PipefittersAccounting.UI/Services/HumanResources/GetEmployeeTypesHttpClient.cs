using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetEmployeeTypesHttpClient
    {
        public static async Task<OperationResult<List<EmployeeTypes>>> Query
        (
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = "1.0/employees/employeetypes";
                using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                    var employeeTypes = await JsonSerializer.DeserializeAsync<List<EmployeeTypes>>(jsonResponse, options);
                    return OperationResult<List<EmployeeTypes>>.CreateSuccessResult(employeeTypes!);
                }
                else
                {
                    return OperationResult<List<EmployeeTypes>>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<EmployeeTypes>>.CreateFailure(ex.Message);
            }
        }
    }
}