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
    public class StockSubscriptionsControllerTests : IntegrationTest
    {
        public StockSubscriptionsControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        { }

        [Fact]
        public async Task GetStockSubscriptions_StockSubscriptionsController_ShouldSucceed()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 10 };

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            List<StockSubscriptionListItem> response = await _client
                .GetFromJsonAsync<List<StockSubscriptionListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/stocksubscriptions/list", queryParams));

            Assert.Equal(7, response.Count);
        }

        [Fact]
        public async Task GetStockSubscriptionDetail_StockSubscriptionsController_ShouldSucceed()
        {
            Guid stockId = new Guid("fb39b013-1633-4479-8186-9f9b240b5727");
            using var response = await _client.GetAsync($"{_urlRoot}/stocksubscriptions/detail/{stockId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var subscription = await JsonSerializer.DeserializeAsync<StockSubscriptionDetails>(jsonResponse, _options);

            Assert.Equal(12000, subscription.SharesIssured);
            Assert.Equal(1.00M, subscription.PricePerShare);
        }

        [Fact]
        public async Task CreateStockSubscription_StockSubscriptionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/stocksubscriptions/create";
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_Create();

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
                    var subscription = await JsonSerializer.DeserializeAsync<StockSubscriptionDetails>(jsonResponse, _options);

                    Assert.Equal(8700, subscription.SharesIssured);
                    Assert.Equal(.95M, subscription.PricePerShare);
                }
            }
        }

        [Fact]
        public async Task EditStockSubscription_StockSubscriptionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/stocksubscriptions/edit";
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();

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
        public async Task DeleteStockSubscription_StockSubscriptionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/stocksubscriptions/delete";
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();

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

        [Fact]
        public async Task GetDividendDeclarationDetails_StockSubscriptionsController_ShouldSucceed()
        {
            Guid dividendId = new Guid("2558ab00-118c-4b67-a6d0-1b9888f841bc");

            using var response = await _client.GetAsync($"{_urlRoot}/stocksubscriptions/dividenddeclaration/{dividendId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var subscription = await JsonSerializer.DeserializeAsync<DividendDeclarationDetails>(jsonResponse, _options);

            Assert.Equal(new DateTime(2022, 2, 1), subscription.StockIssueDate);
        }

        [Fact]
        public async Task GetDividendDeclarationListItems_StockSubscriptionsController_ShouldSucceed()
        {
            GetDividendDeclarationsParameters queryParameters =
                new() { StockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770"), Page = 1, PageSize = 10 };

            var queryParams = new Dictionary<string, string?>
            {
                ["stockId"] = queryParameters.StockId.ToString(),
                ["page"] = queryParameters.Page.ToString(),
                ["pageSize"] = queryParameters.PageSize.ToString()
            };

            List<DividendDeclarationListItem> response = await _client
                .GetFromJsonAsync<List<DividendDeclarationListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/stocksubscriptions/dividenddeclarations", queryParams));

            Assert.Equal(5, response.Count);
        }

        [Fact]
        public async Task CreateDividendDeclaration_StockSubscriptionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/stocksubscriptions/dividenddeclaration/create";
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();

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
                    var dividend = await JsonSerializer.DeserializeAsync<DividendDeclarationDetails>(jsonResponse, _options);

                    Assert.Equal(new DateTime(2022, 6, 1), dividend.DividendDeclarationDate);
                }
            }
        }

        [Fact]
        public async Task EditDividendDeclaration_StockSubscriptionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/stocksubscriptions/dividenddeclaration/edit";
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();

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
        public async Task DeleteDividendDeclaration_StockSubscriptionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/stocksubscriptions/dividenddeclaration/delete";
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();

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