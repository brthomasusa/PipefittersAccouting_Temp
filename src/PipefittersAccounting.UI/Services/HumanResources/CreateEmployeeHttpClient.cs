using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class CreateEmployeeHttpClient
    {
        public static async Task<OperationResult<EmployeeReadModel>> Execute
        (
            EmployeeWriteModel model,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                string uri = "1.0/employees/create";

                var memStream = new MemoryStream();
                await JsonSerializer.SerializeAsync(memStream, model);
                memStream.Seek(0, SeekOrigin.Begin);

                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var requestContent = new StreamContent(memStream))
                {
                    request.Content = requestContent;
                    requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var jsonResponse = await response.Content.ReadAsStreamAsync();
                        var employeeDetails = await JsonSerializer.DeserializeAsync<EmployeeReadModel>(jsonResponse, options);

                        return OperationResult<EmployeeReadModel>.CreateSuccessResult(employeeDetails!);
                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult<EmployeeReadModel>.CreateFailure(ex.Message);
            }
        }
    }
}
