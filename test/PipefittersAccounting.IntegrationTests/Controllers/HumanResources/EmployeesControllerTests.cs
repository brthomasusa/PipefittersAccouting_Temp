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
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.Controllers.HumanResources
{
    [Trait("Integration", "TestServer")]
    public class EmployeesControllerTests : IntegrationTest
    {
        public EmployeesControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        { }

        [Fact]
        public async Task List_ShouldReturnAllEmployees_ShouldSucceed()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 15 };

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            List<EmployeeListItem> response = await _client
                .GetFromJsonAsync<List<EmployeeListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/employees/list", queryParams));

            Assert.Equal(13, response.Count);
        }

        [Fact]
        public async Task ListByName_ShouldReturnAllEmployees_ShouldSucceed()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 15 };

            var queryParams = new Dictionary<string, string?>
            {
                ["LastName"] = "Sanchez",
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            List<EmployeeListItem> response = await _client
                .GetFromJsonAsync<List<EmployeeListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/employees/search", queryParams));

            int count = response.Count;
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task ShouldReturn_AllEmployees_JsonSerializer_DeserializeAsync()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = "1",
                ["pageSize"] = "15"
            };

            using var response = await _client.GetAsync(QueryHelpers.AddQueryString($"{_urlRoot}/employees/list", queryParams),
                                                        HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var employeeListItems = await JsonSerializer.DeserializeAsync<List<EmployeeListItem>>(jsonResponse, _options);

            int count = employeeListItems.Count;
            Assert.Equal(13, count);
        }

        [Fact]
        public async Task Managers_ShouldReturnAllEmployeeManagers_ShouldSucceed()
        {
            using var response = await _client.GetAsync($"{_urlRoot}/employees/managers",
                                                        HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var employeeManagers = await JsonSerializer.DeserializeAsync<List<EmployeeManager>>(jsonResponse, _options);

            Assert.Equal(6, employeeManagers.Count);
        }

        [Fact]
        public async Task Detail_ShouldReturnEmployeeDetail_ShouldSucceed()
        {
            Guid employeeId = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");
            using var response = await _client.GetAsync($"{_urlRoot}/employees/detail/{employeeId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var employeeDetails = await JsonSerializer.DeserializeAsync<EmployeeDetail>(jsonResponse, _options);

            Assert.Equal("Ken", employeeDetails.FirstName);
            Assert.Equal("Sanchez", employeeDetails.LastName);
        }

        [Fact]
        public async Task ShouldCreate_Employee_CreateEmployeeInfo_FromStream()
        {
            string uri = $"{_urlRoot}/employees/create";
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelCreate();

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
                    var employeeDetails = await JsonSerializer.DeserializeAsync<EmployeeDetail>(jsonResponse, _options);
                }
            }
        }

        [Fact]
        public async Task ShouldCreate_Employee_CreateEmployeeInfo_FromString()
        {
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelCreate();
            var createEmployee = JsonSerializer.Serialize(model);
            var requestContent = new StringContent(createEmployee, Encoding.UTF8, "application/json");

            string uri = $"{_urlRoot}/employees/create";
            var response = await _client.PostAsync(uri, requestContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var employeeDetails = JsonSerializer.Deserialize<EmployeeDetail>(content, _options);

            Assert.Equal(model.FirstName, employeeDetails.FirstName);
            Assert.Equal(model.LastName, employeeDetails.LastName);
        }

        [Fact]
        public async Task ShouldCreate_Employee_CreateEmployeeInfo_FromRequestMsg()
        {
            string uri = $"{_urlRoot}/employees/create";
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelCreate();
            var createEmployee = JsonSerializer.Serialize(model);

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(createEmployee, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var employeeDetails = JsonSerializer.Deserialize<EmployeeDetail>(content, _options);

            Assert.Equal(model.FirstName, employeeDetails.FirstName);
            Assert.Equal(model.LastName, employeeDetails.LastName);
        }

        [Fact]
        public async Task ShouldEdit_Employee_EditEmployeeInfo_FromStream()
        {
            string uri = $"{_urlRoot}/employees/edit";
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();
            model.LastName = "Banski";
            model.FirstName = "Rhys";

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
                }
            }
        }

        [Fact(Skip = "No deletable employees")]
        public async Task ShouldDelete_Employee_DeleteEmployeeInfo_FromStream()
        {
            string uri = $"{_urlRoot}/employees/delete";
            EmployeeWriteModel model = EmployeeAggregateTestData.GetEmployeeWriteModelEdit();

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

        [Fact]
        public async Task list_TimeCardsController_ShouldSucceed()
        {
            Guid employeeId = new Guid("6d7f6605-567d-4b2a-9ae7-3736dc6c4f53");
            using var response = await _client.GetAsync($"{_urlRoot}/employees/timecards/{employeeId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var timeCards = await JsonSerializer.DeserializeAsync<List<TimeCardListItem>>(jsonResponse, _options);

            int count = 2;
            Assert.Equal(count, timeCards.Count);
        }

        [Fact]
        public async Task GetPayrollRegister_TimeCardsController_ShouldSucceed()
        {
            DateTime payPeriodDate = new(2022, 2, 28);

            var queryParams = new Dictionary<string, string?>
            {
                ["payPeriodEnded"] = payPeriodDate.ToShortDateString()
            };

            List<PayrollRegister> response = await _client
                .GetFromJsonAsync<List<PayrollRegister>>(QueryHelpers.AddQueryString($"{_urlRoot}/employees/timecards/payrollregister", queryParams));

            Assert.Equal(13, response.Count);
        }

        [Fact]
        public async Task detail_TimeCardsController_ShouldSucceed()
        {
            Guid timeCardId = new Guid("748fcab5-9464-4d5f-937f-d61ffe811e6f");
            using var response = await _client.GetAsync($"{_urlRoot}/employees/timecard/{timeCardId}",
                                                        HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var timecard = await JsonSerializer.DeserializeAsync<TimeCardDetail>(jsonResponse, _options);

            Assert.Equal(168, timecard.RegularHours);
            Assert.Equal(4, timecard.OvertimeHours);
        }

        [Fact]
        public async Task CreateTimeCard_TimeCardsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/employees/timecard/create";
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForCreate();

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
                    var timeCard = await JsonSerializer.DeserializeAsync<TimeCardDetail>(jsonResponse, _options);

                    Assert.Equal(new DateTime(2022, 3, 31), timeCard.PayPeriodEnded);
                }
            }
        }

        [Fact]
        public async Task EditTimeCard_TimeCardsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/employees/timecard/edit";
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForEdit();

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
        public async Task DeleteTimeCard_TimeCardsController_ShouldSucceed()
        {
            string uri = $"{_urlRoot}/employees/timecard/delete";
            TimeCardWriteModel model = EmployeeAggregateTestData.GetTimeCardForEdit();

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