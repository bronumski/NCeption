using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NCeption.SqlServer;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.Data.SqlServer
{
    class When_setting_up_a_test_sql_database
    {
        private TestSqlServerDatabase sqlServerDeploy;

        private INCeptionConfiguration configuration;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            sqlServerDeploy = new TestSqlServerDatabase();
            
            configuration = Substitute.For<INCeptionConfiguration>();
            configuration.TestCorrelationId.Returns(Guid.NewGuid().ToString("N"));
            configuration.TestSuiteName.Returns("FOO");

            sqlServerDeploy.Deploy(configuration);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            sqlServerDeploy.Delete();
        }

        [Test]
        public void Should_create_database_with_unique_name()
        {
            var server = new Server(new ServerConnection(sqlServerDeploy.CreateConnection()));

            server.Databases.Cast<Database>().Should().Contain(x => x.Name == string.Format("{0}_{1}_{2}", "FOO", configuration.TestCorrelationId, typeof(TestSqlServerDatabase).FullName));
        }

        [Test]
        public void Should_be_able_to_query_table_on_database()
        {
            using (SqlConnection connection = sqlServerDeploy.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = "select * from Wibble";

                using (var reader = command.ExecuteReader())
                {
                    reader.GetName(0).Should().Be("Wobble");
                }
            }
        }
    }

    class TestSqlServerDatabase : SqlServerDatabaseProject
    {
        protected override string DatabaseProjectPath
        {
            get { return @"..\..\..\TestProjects\DatabaseTestProject\DatabaseTestProject.sqlproj"; }
        }
    }
}