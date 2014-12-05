using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NCeption.Data;

namespace NCeption.SqlServer
{
    public abstract class SqlServerDatabaseProject : IDatabaseDeployer
    {
        private string databaseName;

        public void Deploy(INCeptionConfiguration configuration)
        {
            using (var projectCollection = new ProjectCollection())
            {
                var connectionStringBuilder = CreateConnectionStringBuilder(x => { });

                databaseName = string.Format("{0}_{1}_{2}", configuration.TestSuiteName, configuration.TestCorrelationId,
                    GetType().FullName);

                var globalProperty = new Dictionary<string, string>
                {
                    {"TargetDatabase", databaseName},
                    {"DeployDatabase", "True"},
                    {"TargetConnectionString", connectionStringBuilder.ConnectionString}
                };

                var buildParameters = new BuildParameters(projectCollection)
                {
                    Loggers = new[] { new ConsoleLogger() },
                    DefaultToolsVersion = "4.0"
                };

                var buildRequestData = new BuildRequestData(DatabaseProjectPath, globalProperty, "4.0", new[] { "Build", "Deploy" }, null);

                var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequestData);

                if (buildResult.OverallResult != BuildResultCode.Success)
                {
                    throw new Exception(string.Format("Failed to deploy Sql Server Database Project '{0}':/n{1}", DatabaseProjectPath, buildResult.Exception));
                }
            }
        }

        private SqlConnectionStringBuilder CreateConnectionStringBuilder(Action<SqlConnectionStringBuilder> build)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = DataSource,
                IntegratedSecurity = true,
                Pooling = false
            };

            build(connectionStringBuilder);

            return connectionStringBuilder;
        }

        protected virtual string DataSource
        {
            get { return "LocalHost"; }
        }

        public void Delete()
        {
            var server = new Server(new ServerConnection(CreateConnection()));

            server.KillDatabase(databaseName);
        }

        public void DeleteOrphanedDataStores(INCeptionConfiguration configuration)
        {
            var server = new Server(new ServerConnection(CreateConnection()));

            Parallel.ForEach(server.Databases.Cast<Database>().Where(x => x.Name.StartsWith(configuration.TestSuiteName) && x.CreateDate > DateTime.Now - configuration.OrphanStaleness),
                database => server.KillDatabase(database.Name));
        }

        protected abstract string DatabaseProjectPath { get; }

        public SqlConnection CreateConnection()
        {
            var connectionStringBuilder = CreateConnectionStringBuilder(x => x.InitialCatalog = databaseName);

            var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}