using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.Data.DatabaseManagerFixtures
{
    class When_deploying_databases
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseManager.Current.DeleteAll();
        }

        [Test]
        public void Should_only_deploy_once()
        {
            var databaseThing = Substitute.For<IDatabaseDeployer>();

            DatabaseManager.Current.Deploy(databaseThing);

            DatabaseManager.Current.Deploy(databaseThing);

            databaseThing.Received(1).Deploy(Arg.Any<INCeptionConfiguration>());
        }

        [Test]
        public void Should_delete_orphaned_data_stores_only_once()
        {
            var databaseThing = Substitute.For<IDatabaseDeployer>();

            DatabaseManager.Current.Deploy(databaseThing);

            DatabaseManager.Current.Deploy(databaseThing);

            databaseThing.Received(1).DeleteOrphanedDataStores(Arg.Any<INCeptionConfiguration>());
        }

        [Test]
        public void Should_delete_database()
        {
            var databaseThing = new Foo();

            DatabaseManager.Current.Deploy(databaseThing);

            DatabaseManager.Current.Delete<Foo>();

            databaseThing.Deleted.Should().BeTrue();
        }

        [Test]
        public void Should_return_the_deployed_database_instance()
        {
            var databaseThing = new Foo();

            DatabaseManager.Current.Deploy(databaseThing);

            DatabaseManager.Current.Database<Foo>().Should().BeSameAs(databaseThing);
        }

        [Test]
        public void Should_swallow_exceptions_when_deleting()
        {
            var databaseThing = new DatabaseDeleteException();

            DatabaseManager.Current.Deploy(databaseThing);

            Action act = () => DatabaseManager.Current.Delete<DatabaseDeleteException>();

            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public void Should_throw_exception_if_database_is_not_found_when_deleting()
        {
            Action act = () => DatabaseManager.Current.Delete<Foo>();

            act
                .ShouldThrow<DeployedDatabaseNotFoundException>()
                .WithMessage(string.Format("Could not delete deployed database '{0}'", typeof(Foo)));
        }

        [Test]
        public void Should_throw_exception_if_database_is_not_found_when_getting_the_deployed_database()
        {
            Action act = () => DatabaseManager.Current.Database<Foo>();

            act
                .ShouldThrow<DeployedDatabaseNotFoundException>()
                .WithMessage(string.Format("Could not delete deployed database '{0}'", typeof(Foo)));
        }
        
        private class Foo : IDatabaseDeployer
        {
            public bool Deleted { get; private set; }
            public void Deploy(INCeptionConfiguration configuration)
            {
            }

            public void Delete()
            {
                Deleted = true;
            }

            public void DeleteOrphanedDataStores(INCeptionConfiguration configuration)
            {
                
            }
        }

        private class DatabaseDeleteException : IDatabaseDeployer {
            public void Deploy(INCeptionConfiguration configuration)
            {
                
            }

            public void Delete()
            {
                throw new Exception();
            }

            public void DeleteOrphanedDataStores(INCeptionConfiguration configuration)
            {
                
            }
        }
    }

}