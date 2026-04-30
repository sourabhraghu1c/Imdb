using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

namespace IMDBSample.Repository
{
    public class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository(IOptions<ConnectionString> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return connection.Query<T>(sql, param);
        }

        protected T QuerySingle<T>(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<T>(sql, param);
        }

        protected int Execute(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return connection.Execute(sql, param);
        }

        protected T ExecuteScalar<T>(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return connection.ExecuteScalar<T>(sql, param);
        }

        protected int ExecuteSp(string sp, object param = null)
        {
            using var connection = CreateConnection();
            return connection.Execute(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        protected T ExecuteScalarSp<T>(string sp, object param = null)
        {
            using var connection = CreateConnection();
            return connection.ExecuteScalar<T>(
                sp,
                param,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}