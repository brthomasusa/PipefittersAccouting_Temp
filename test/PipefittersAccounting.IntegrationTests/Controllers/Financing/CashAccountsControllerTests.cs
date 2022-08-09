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
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.IntegrationTests.Controllers.Financing
{
    public class CashAccountsControllerTests : IntegrationTest
    {
        public CashAccountsControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        { }

        [Fact]
        public async Task GetCashAccounts_CashAccountsController_ShouldSucceed()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 10 };

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            List<CashAccountListItem> response = await _client
                .GetFromJsonAsync<List<CashAccountListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/cashaccounts/list", queryParams));

            Assert.Equal(4, response.Count);
        }

        [Fact]
        public async Task GetCashAccount_CashAccountsController_ShouldSucceed()
        {
            Guid cashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e");
            using var response = await _client.GetAsync($"{_urlRoot}/cashaccounts/detail/{cashAccountId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var cashAccount = await JsonSerializer.DeserializeAsync<CashAccountDetail>(jsonResponse, _options);

            Assert.Equal(35625M, cashAccount.Balance);
        }

        [Fact]
        public async Task CreateCashAccount_CashAccountsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/create";
            CashAccountWriteModel model = CashAccountTestData.GetCreateCashAccountInfo();

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
                    var cashAccount = await JsonSerializer.DeserializeAsync<CashAccountDetail>(jsonResponse, _options);

                    Assert.Equal(0, cashAccount.Balance);
                    Assert.Equal("Big Bank", cashAccount.BankName);
                }
            }
        }

        [Fact]
        public async Task EditCashAccount_CashAccountsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/edit";
            CashAccountWriteModel model = CashAccountTestData.GetEditCashAccountInfo();

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
        public async Task DeleteCashAccount_CashAccountsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/delete";
            CashAccountWriteModel model = new() { CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76") };

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

                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                }
            }
        }
    }
}