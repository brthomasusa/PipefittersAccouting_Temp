using System.Net.Http.Headers;
using System.Text.Json;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.HumanResources
{
    public class DeleteEmployeeHttpClient
    {
        public static async Task<OperationResult<bool>> Execute
        (
            EmployeeWriteModel model,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                string uri = "1.0/employees/delete";

                var memStream = new MemoryStream();
                await JsonSerializer.SerializeAsync(memStream, model);
                memStream.Seek(0, SeekOrigin.Begin);

                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var requestContent = new StreamContent(memStream))
                {
                    request.Content = requestContent;
                    requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var jsonResponse = await response.Content.ReadAsStreamAsync();
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}