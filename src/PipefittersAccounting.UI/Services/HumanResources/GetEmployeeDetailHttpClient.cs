using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class GetEmployeeDetailHttpClient
    {
        public static async Task<OperationResult<EmployeeDetail>> Query
        (
            GetEmployeeParameter queryParameters,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                var uri = $"1.0/employees/detail/{queryParameters.EmployeeID}";
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    EmployeeDetail? employeeDetail = JsonSerializer.Deserialize<EmployeeDetail>(content, options);
                    if (employeeDetail is not null)
                    {
                        return OperationResult<EmployeeDetail>.CreateSuccessResult(employeeDetail);
                    }
                    else
                    {
                        return OperationResult<EmployeeDetail>.CreateFailure($"Employee with Id: '{queryParameters.EmployeeID}' was not found.");
                    }
                }
                else
                {
                    return OperationResult<EmployeeDetail>.CreateFailure($"Status code: {response.StatusCode.ToString()}");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<EmployeeDetail>.CreateFailure(ex.Message);
            }
        }
    }
}