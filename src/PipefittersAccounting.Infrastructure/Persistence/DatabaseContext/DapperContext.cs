using System.Data;
using Microsoft.Data.SqlClient;

namespace PipefittersAccounting.Infrastructure.Persistence.DatabaseContext
{
    public class DapperContext
    {
        private readonly string _connectionStr;

        public DapperContext(string connStr) => _connectionStr = connStr;

        public IDbConnection CreateConnection() => new SqlConnection(_connectionStr);
    }
}