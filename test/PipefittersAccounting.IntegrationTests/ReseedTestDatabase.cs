using System;
using System.Data;
using System.Data.SqlClient;

namespace PipefittersAccounting.IntegrationTests
{
    public class ReseedTestDatabase
    {
        public static void ReseedDatabase()
        {
            string connectionString = "Server=tcp:mssql-server,1433;Database=Pipefitters_Test;User Id=sa;Password=Info99Gum;MultipleActiveResultSets=true;TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_resetTestDb", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    cmd.BeginExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}