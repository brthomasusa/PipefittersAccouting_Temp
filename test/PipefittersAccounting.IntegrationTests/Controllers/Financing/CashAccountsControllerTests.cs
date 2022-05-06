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
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.IntegrationTests.Base;
using PipefittersAccounting.SharedModel.Readmodels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

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

            Assert.Equal(2, response.Count);
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

            Assert.Equal(20000M, cashAccount.Balance);
        }

        [Fact]
        public async Task CreateCashAccount_CashAccountsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/create";
            CreateCashAccountInfo model = GetCreateCashAccountInfo();

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
            EditCashAccountInfo model = GetEditCashAccountInfo();

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
            DeleteCashAccountInfo model = new() { CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76") };

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

        private CreateCashAccountInfo GetCreateCashAccountInfo()
            => new CreateCashAccountInfo
            {
                CashAccountId = new Guid("210d34d7-7474-44e7-a90b-93998137917a"),
                CashAccountType = 2,
                BankName = "Big Bank",
                CashAccountName = "Party Party Party!",
                CashAccountNumber = "123456789",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 3),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };

        private EditCashAccountInfo GetEditCashAccountInfo()
            => new EditCashAccountInfo
            {
                CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76"),
                CashAccountType = 2,
                BankName = "Big Bank",
                CashAccountName = "Party Party Party!",
                RoutingTransitNumber = "987654321",
                DateOpened = new DateTime(2022, 5, 3),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
    }
}