using System.Data;
using System.Data.SqlClient;

namespace Gupshupcampainmanager.Persistence
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
