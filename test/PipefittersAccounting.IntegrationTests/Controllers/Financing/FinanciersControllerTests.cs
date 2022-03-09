#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
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
    public class FinanciersControllerTests : IntegrationTest
    {
        public FinanciersControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        { }

        [Fact]
        public async Task GetFinanciers_ReturnAllFinancier_ShouldSucceed()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 10 };

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            List<FinancierListItems> response = await _client
                .GetFromJsonAsync<List<FinancierListItems>>(QueryHelpers.AddQueryString($"{_urlRoot}/financiers/list", queryParams));

            Assert.Equal(6, response.Count);
        }

        [Fact]
        public async Task GetFinancierDetail_ReturnsOneFinancierDetail_ShouldSucceed()
        {
            Guid financierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            using var response = await _client.GetAsync($"{_urlRoot}/financiers/detail/{financierId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var financierDetail = await JsonSerializer.DeserializeAsync<FinancierDetail>(jsonResponse, _options);

            Assert.Equal("Arturo Sandoval", financierDetail.FinancierName);
        }

        [Fact]
        public async Task CreateFinancierInfo_CreateFinancierWithValidInfo_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/financiers/create";
            CreateFinancierInfo model = TestUtilities.GetCreateFinancierInfo();

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
                    var financierDetails = await JsonSerializer.DeserializeAsync<FinancierDetail>(jsonResponse, _options);

                    Assert.Equal(model.FinancierName, financierDetails.FinancierName);
                }
            }
        }

        [Fact]
        public async Task EditFinancierInfo_EditFinancierWithValidInfo_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/financiers/edit";
            EditFinancierInfo model = TestUtilities.GetEditFinancierInfo();

            var memStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memStream, model);
            memStream.Seek(0, SeekOrigin.Begin);

            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var requestContent = new StreamContent(memStream))
            {
                request.Content = requestContent;
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                }
            }
        }

        [Fact]
        public async Task DeleteFinancierInfo_DeleteFinancierWithValidId_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/financiers/delete";
            DeleteFinancierInfo model = TestUtilities.GetDeleteFinancierInfo();

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