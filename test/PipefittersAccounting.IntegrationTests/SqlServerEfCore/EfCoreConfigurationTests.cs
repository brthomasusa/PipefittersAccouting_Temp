using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PipefittersAccounting.Core.HumanResources;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore
{
    public class EfCoreConfigurationTests : TestBaseEfCore
    {
        public EfCoreConfigurationTests() => _dbContext.Database.ExecuteSqlRaw("EXEC dbo.usp_resetTestDb");

        [Fact]
        public void ShouldReturnAt_ExternalAgentEmployees()
        {
            List<ExternalAgent> agents = _dbContext.ExternalAgents.AsNoTracking()
                                                                  .Where(e => e.AgentType == AgentTypeEnum.Employee)
                                                                  .ToList();
            int count = agents.Count;

            Assert.True(count >= 9);
        }

        [Fact]
        public void ShouldReturn_Employees()
        {
            List<Employee> employees = _dbContext.Employees
                    .AsNoTracking()
                    .ToList();

            int count = employees.Count;

            Assert.True(count >= 9);
        }
    }
}