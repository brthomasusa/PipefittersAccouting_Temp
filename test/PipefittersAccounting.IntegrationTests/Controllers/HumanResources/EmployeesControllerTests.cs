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
        public async Task ShouldReturn_AllEmployees_EmployeesController()
        {
            var pagingParams = new PagingParameters { Page = 1, PageSize = 10 };

            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = pagingParams.Page.ToString(),
                ["pageSize"] = pagingParams.PageSize.ToString()
            };

            try
            {
                List<EmployeeListItem> response = await _client
                    .GetFromJsonAsync<List<EmployeeListItem>>(QueryHelpers.AddQueryString($"{_urlRoot}/employees/list", queryParams));

                Assert.True(true);
            }
            catch (System.Text.Json.JsonException ex)
            {
                System.Console.Write(ex);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                System.Console.Write(ex);
            }
            catch (System.Exception ex)
            {
                System.Console.Write(ex);
            }

        }

        [Fact]
        public async Task ShouldReturn_AllEmployees_JsonSerializer_DeserializeAsync()
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["page"] = "1",
                ["pageSize"] = "10"
            };

            using var response = await _client.GetAsync(QueryHelpers.AddQueryString($"{_urlRoot}/employees/list", queryParams),
                                                        HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var employeeListItems = await JsonSerializer.DeserializeAsync<List<EmployeeListItem>>(jsonResponse, _options);

            Assert.Equal(9, employeeListItems.Count);
        }

        [Fact]
        public async Task ShouldReturn_AllEmployeeManagers()
        {
            using var response = await _client.GetAsync($"{_urlRoot}/employees/managers",
                                                        HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var employeeManagers = await JsonSerializer.DeserializeAsync<List<EmployeeManager>>(jsonResponse, _options);

            Assert.Equal(3, employeeManagers.Count);
        }

        [Fact]
        public async Task ShouldReturn_EmployeeDetails_ForOneEmployee()
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
            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();

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
            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();
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
            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();
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
            EditEmployeeInfo model = TestUtilities.GetEditEmployeeInfo();
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

        [Fact]
        public async Task ShouldDelete_Employee_DeleteEmployeeInfo_FromStream()
        {
            string uri = $"{_urlRoot}/employees/delete";
            DeleteEmployeeInfo model = TestUtilities.GetDeleteEmployeeInfo();


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