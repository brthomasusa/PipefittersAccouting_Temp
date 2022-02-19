using Microsoft.Extensions.Configuration;
using TestSupport.Helpers;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper
{
    public class TestBaseDapper
    {
        protected readonly DapperContext _dapperCtx;
        protected readonly string serviceAddress = "https://localhost:7035/";

        public TestBaseDapper()
        {
            var config = AppSettings.GetConfiguration();
            _dapperCtx = new DapperContext(config.GetConnectionString("DefaultConnection"));
        }
    }
}