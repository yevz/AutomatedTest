using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using AutomatedTest.Database.Extensions;
using Dapper;

namespace AutomatedTest.Database.Database
{
    public class Database
    {
        private static string _connectionString;
        private static readonly Dictionary<string, string> ResultCache = new Dictionary<string, string>();


        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static IDbConnection GetConnection(string sqlSource, int connectionTimeout = 120)
        {
            var connectionString = ConfigurationManager.
                ConnectionStrings[$"{sqlSource.ToUpperInvariant()}.DBConnString"].ConnectionString;
            var connection = new OdbcConnection(connectionString) { ConnectionTimeout = connectionTimeout };
            connection.Open();
            return connection;
        }

        public string ExecuteSQL(string sqlQuery, string sqlSource = "")
        {
            string result;

            if (ResultCache.ContainsKey(sqlQuery))
            {
                return ResultCache[sqlQuery];
            }

            using (var con = GetConnection(sqlSource))
            {
                result = con.ExecuteScalar(sqlQuery).ToString();
            }
            ResultCache[sqlQuery] = result;
            return result;

        }
    }
}
