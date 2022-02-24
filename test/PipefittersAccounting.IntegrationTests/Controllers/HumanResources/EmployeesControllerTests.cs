#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.Controllers.HumanResources
{
    public class EmployeesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        readonly HttpClient _client;

        public EmployeesControllerTests(WebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
            ReseedTestDatabase.ReseedDatabase();
        }


        [Fact]
        public async Task WebApplicationFactory_Setup_Test()
        {
            var response = await _client.GetAsync("api/v1/employees");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // [Fact]
        // public async Task ShouldReturn_AllEmployees()
        // {
        //     // Arrange


        //     // Act


        //     // Assert            

        // }

    }
}