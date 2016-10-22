using Me.DataLayer.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Me.DataLayer.Util
{
    public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DapperContext()
        {
            _connectionString = ConfigurationSettings.GetConnectionString();
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }
    }
}
