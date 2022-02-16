#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.WebApi.Controllers.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;


namespace PipefittersAccounting.IntegrationTests.Sqlite.Controllers.HumanResources
{
    public class EmployeesControllerTests
    {
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesControllerTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _logger = factory.CreateLogger<EmployeesController>();
        }

        [Fact]
        public void TestLoggingInjection_Into_Controller()
        {
            // EmployeesController controller = new EmployeesController(_logger);

            // Assert.NotNull(controller);
        }
    }
}