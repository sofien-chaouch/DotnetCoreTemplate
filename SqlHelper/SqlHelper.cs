using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using PlatformService.Interfaces;
using PlatformService.Interfaces;

namespace PlatformService.SqlHelper
{
    public class SqlHelper : ISqlHelper
    {
        private readonly IConfiguration _configuration;

        public SqlHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                NpgsqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;
                connection.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dt.TableName = "result";
                dt.Load(reader);
                if (reader != null)
                {
                    reader.Dispose();
                }
                connection.Close();
            }
            return dt;
        }
    }
}
