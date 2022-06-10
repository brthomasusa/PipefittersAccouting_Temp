#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;
using PipefittersAccounting.IntegrationTests.Base;
using PipefittersAccounting.SharedModel.Readmodels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.IntegrationTests.Controllers.Financing
{
    [Trait("Integration", "TestServer")]
    public class LoanAgreementsControllerTests : IntegrationTest
    {
        public LoanAgreementsControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        { }

        [Fact]
        public async Task GetLoanAgreements_LoanAgreementsController_ShouldSucceed()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 10 };

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            List<LoanAgreementListItem> response = await _client
                .GetFromJsonAsync<List<LoanAgreementListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/loanagreements/list", queryParams));

            Assert.Equal(4, response.Count);
        }

        [Fact]
        public async Task GetLoanAgreementDetail_LoanAgreementsController_ShouldSucceed()
        {
            Guid loanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1");
            using var response = await _client.GetAsync($"{_urlRoot}/loanagreements/detail/{loanId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var detail = await JsonSerializer.DeserializeAsync<LoanAgreementDetail>(jsonResponse, _options);

            Assert.Equal("Arturo Sandoval", detail.FinancierName);
        }

        [Fact]
        public async Task CreateLoanAgreement_LoanAgreementsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/loanagreements/create";
            LoanAgreementWriteModel model = LoanAgreementTestData.GetCreateLoanAgreementInfo();

            var memStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memStream, model);
            memStream.Seek(0, SeekOrigin.Begin);

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var requestContent = new StreamContent(memStream))
            {
                request.Content = requestContent;
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                    var details = await JsonSerializer.DeserializeAsync<LoanAgreementDetail>(jsonResponse, _options);

                    Assert.Equal(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"), model.FinancierId);
                }
            }
        }

        [Fact]
        public async Task DeleteLoanAgreement_LoanAgreementsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/loanagreements/delete";

            DeleteLoanAgreementInfo model = new DeleteLoanAgreementInfo
            {
                LoanId = new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867"),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

            var memStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memStream, model);
            memStream.Seek(0, SeekOrigin.Begin);

            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var requestContent = new StreamContent(memStream))
            {
                request.Content = requestContent;
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}