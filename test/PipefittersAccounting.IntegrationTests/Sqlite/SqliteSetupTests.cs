using System.Linq;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.Sqlite
{
    public class SqliteSetupTests
    {
        private readonly ITestOutputHelper _output;

        public SqliteSetupTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void TestSqlite_AgentTypesPrimaryKeyNumbering()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            context.ExternalAgentTypes.First().Id.ShouldEqual(1);
            context.ExternalAgentTypes.First().AgentTypeName.ShouldEqual("Customer");

            var query = context.ExternalAgentTypes.Find(6) ?? throw new System.Exception("Null query results");
            Assert.Equal("Financier", query.AgentTypeName);
        }

        [Fact]
        public void TestSqlite_EventTypesPrimaryKeyNumbering()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            context.EconomicEventTypes.First().Id.ShouldEqual(1);
            context.EconomicEventTypes.First().EventTypeName.ShouldEqual("Cash Receipt from Sales");

            var query = context.EconomicEventTypes.Find(7) ?? throw new System.Exception("Null query results");
            Assert.Equal("Cash Disbursement for Inventory Receipt", query.EventTypeName);
        }

        [Fact]
        public void TestSqlite_ExternalAgents()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            var query = context.ExternalAgents.ToList();

            query.Count.ShouldEqual(15);
        }

        [Fact]
        public void TestSqlite_InsertEmployees()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            var query = context.Employees.ToList();

            query.Count.ShouldEqual(9);
        }

        [Fact]
        public void TestSqlite_InsertDomainUser()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            var query = context.DomainUsers.ToList();

            query.Count.ShouldEqual(1);
        }

        [Fact]
        public void TestSqlite_InsertFinanciers()
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            var query = context.Financiers.ToList();

            query.Count.ShouldEqual(6);
        }
    }
}