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
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.IntegrationTests.Controllers.Financing
{
    public class CashAccountTransactionsControllerTests : IntegrationTest
    {
        public CashAccountTransactionsControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        { }

        [Fact]
        public async Task GetCashAccountTransactionListItems_CashAccountTransactionsController_ShouldSucceed()
        {
            Guid cashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e");
            int page = 1;
            int pageSize = 10;

            var queryParams = new Dictionary<string, string?>
            {
                ["cashAccountId"] = cashAccountId.ToString(),
                ["page"] = page.ToString(),
                ["pageSize"] = pageSize.ToString()
            };

            List<CashAccountTransactionListItem> response = await _client
                .GetFromJsonAsync<List<CashAccountTransactionListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/cashaccounts/cashtransactions", queryParams));

            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task GetCashAccountTransactionDetails_CashAccountTransactionsController_ShouldSucceed()
        {
            int cashTransactionId = 1;

            using var response = await _client.GetAsync($"{_urlRoot}/cashaccounts/cashtransaction/{cashTransactionId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var cashTransaction = await JsonSerializer.DeserializeAsync<CashAccountTransactionDetail>(jsonResponse, _options);

            Assert.Equal(10000M, cashTransaction.CashAcctTransactionAmount);
        }

        [Fact]
        public async Task CreateDepositForDebtIssueProceeds_CashAccountTransactionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/cashtransaction/createdeposit";
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();

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
                    var cashTransaction = await JsonSerializer.DeserializeAsync<CashAccountTransactionDetail>(jsonResponse, _options);

                    Assert.Equal(4000M, cashTransaction.CashAcctTransactionAmount);
                }
            }
        }

        [Fact]
        public async Task CreateDepositForStockIssueProceeds_CashAccountTransactionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/cashtransaction/createdeposit";
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();

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
                    var cashTransaction = await JsonSerializer.DeserializeAsync<CashAccountTransactionDetail>(jsonResponse, _options);

                    Assert.Equal(5985M, cashTransaction.CashAcctTransactionAmount);
                }
            }
        }

        [Fact]
        public async Task CreateDisbursementForLoanInstallmentPayment_CashAccountTransactionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/cashtransaction/createdisbursement";
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();

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
                    var cashTransaction = await JsonSerializer.DeserializeAsync<CashAccountTransactionDetail>(jsonResponse, _options);

                    Assert.Equal(1370.54M, cashTransaction.CashAcctTransactionAmount);
                }
            }
        }

        [Fact]
        public async Task CreateDisbursementForDividendPayment_CashAccountTransactionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/cashtransaction/createdisbursement";
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();

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
                    var cashTransaction = await JsonSerializer.DeserializeAsync<CashAccountTransactionDetail>(jsonResponse, _options);

                    Assert.Equal(100.00M, cashTransaction.CashAcctTransactionAmount);
                }
            }
        }

        [Fact]
        public async Task CreateCashTransfer_CashAccountTransactionsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/cashaccounts/cashtransaction/createtransfer";
            CashAccountTransferWriteModel model = CashAccountTestData.GetCreateCashAccountTransferInfo();

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
                }
            }
        }

    }
}