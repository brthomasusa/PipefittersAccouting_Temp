using System.Net.Http.Headers;
using System.Text.Json;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Services.Finance
{
    public class CreateLoanAgreementHttpClient
    {
        public static async Task<OperationResult<LoanAgreementDetail>> Execute
        (
            LoanAgreementWriteModel model,
            HttpClient client,
            JsonSerializerOptions options
        )
        {
            try
            {
                string uri = "1.0/loanagreements/create";

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
                        var loanDetails = await JsonSerializer.DeserializeAsync<LoanAgreementDetail>(jsonResponse, options);

                        return OperationResult<LoanAgreementDetail>.CreateSuccessResult(loanDetails!);
                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult<LoanAgreementDetail>.CreateFailure(ex.Message);
            }
        }
    }
}