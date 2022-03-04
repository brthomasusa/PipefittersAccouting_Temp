using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using TestSupport.Helpers;
using Xunit;

using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper
{
    [Trait("Integration", "DapperQuerySvc")]
    public class TestBaseDapper
    {
        protected readonly DapperContext _dapperCtx;
        protected readonly string serviceAddress = "https://localhost:7035/";

        public TestBaseDapper()
        {
            var config = AppSettings.GetConfiguration();
            _dapperCtx = new DapperContext(config.GetConnectionString("DefaultConnection"));

            using (var connection = _dapperCtx.CreateConnection())
            {
                connection.Execute("dbo.usp_resetTestDb", commandType: CommandType.StoredProcedure);
            }
        }
    }
}
