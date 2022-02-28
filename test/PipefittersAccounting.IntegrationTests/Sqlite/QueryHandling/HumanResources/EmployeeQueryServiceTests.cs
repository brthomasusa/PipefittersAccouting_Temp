#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Application.Commands.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;


using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;


namespace PipefittersAccounting.IntegrationTests.Sqlite.QueryHandling.HumanResources
{
    public class EmployeeQueryServiceTests
    {
        [Fact]
        public void ShouldReturnValid_EmployeeQueryService_Sqlite()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            IEmployeeAggregateQueryService sqliteQrySvc = new EmployeeQueryServiceSqlite(context);

            Assert.NotNull(sqliteQrySvc);
        }

        [Fact]
        public async Task TestQueryService_GetEmployeeDetail()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            IEmployeeAggregateQueryService sqliteQrySvc = new EmployeeQueryServiceSqlite(context);
            Guid agentID = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");
            GetEmployee qryParam = new GetEmployee() { EmployeeID = agentID };

            OperationResult<EmployeeDetail> returnValue = await sqliteQrySvc.GetEmployeeDetails(qryParam);

            Assert.True(returnValue.Success);
            Assert.IsType<EmployeeDetail>(returnValue.Result);
            Assert.Equal(agentID, returnValue.Result.EmployeeId);
        }

        [Fact]
        public async Task TestQueryService_GetEmployeeListItems()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            IEmployeeAggregateQueryService sqliteQrySvc = new EmployeeQueryServiceSqlite(context);

            GetEmployees qryParam = new GetEmployees() { Page = 0, PageSize = 10 };

            OperationResult<PagedList<EmployeeListItem>> returnValue = await sqliteQrySvc.GetEmployeeListItems(qryParam);

            Assert.True(returnValue.Success);
            Assert.IsType<PagedList<EmployeeListItem>>(returnValue.Result);
            Assert.Equal(9, returnValue.Result.Count);
        }

        [Fact]
        public async Task TestQueryService_GetEmployeeListItems_Pagination()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            IEmployeeAggregateQueryService sqliteQrySvc = new EmployeeQueryServiceSqlite(context);

            GetEmployees qryParam = new GetEmployees() { Page = 0, PageSize = 5 };

            OperationResult<PagedList<EmployeeListItem>> returnValue = await sqliteQrySvc.GetEmployeeListItems(qryParam);

            Assert.True(returnValue.Success);
            Assert.IsType<PagedList<EmployeeListItem>>(returnValue.Result);
            Assert.Equal(5, returnValue.Result.Count);
        }

        [Fact]
        public async Task TestQueryService_GetEmployeeManagers()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            IEmployeeAggregateQueryService sqliteQrySvc = new EmployeeQueryServiceSqlite(context);

            GetEmployeeManagers queryParameters = new GetEmployeeManagers { };

            OperationResult<List<EmployeeManager>> returnValue = await sqliteQrySvc.GetEmployeeManagers(queryParameters);

            Assert.True(returnValue.Success);
            Assert.IsType<List<EmployeeManager>>(returnValue.Result);
            Assert.Equal(3, returnValue.Result.Count);
        }
    }
}