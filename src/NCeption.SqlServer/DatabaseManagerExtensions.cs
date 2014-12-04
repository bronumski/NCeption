using System.Data.SqlClient;
using NCeption.Data;

namespace NCeption.SqlServer
{
    public static class DatabaseManagerExtensions
    {
        public static SqlConnection SqlConnectionFor<T>(this DatabaseManager databaseManager) where T : SqlServerDatabaseProject
        {
            return ((SqlServerDatabaseProject)databaseManager.Database<T>()).CreateConnection();
        }
    }
}