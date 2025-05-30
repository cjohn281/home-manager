using Microsoft.Extensions.Configuration;

namespace home_manager.Data
{
    /// <summary>
    /// Provides centralized access to database connection strings.
    /// </summary>
    public class DbConnectionService
    {
        private readonly string _defaultConnection;

        public DbConnectionService(IConfiguration configuration)
        {
            _defaultConnection = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        /// <summary>
        /// Gets the default database connection string.
        /// </summary>
        /// <returns>The connection string, or empty string if not configured.</returns>
        public string GetConnectionString()
        {
            return _defaultConnection;
        }
    }
}