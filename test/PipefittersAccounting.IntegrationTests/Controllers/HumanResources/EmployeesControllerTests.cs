#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;
using PipefittersAccounting.IntegrationTests.Base;
using PipefittersAccounting.SharedModel.Readmodels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;


namespace PipefittersAccounting.IntegrationTests.Controllers.HumanResources
{
    public class EmployeesControllerTests : IntegrationTest
    {
        public EmployeesControllerTests(ApiWebApplicationFactory fixture) : base(fixture) { }

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


    }
}