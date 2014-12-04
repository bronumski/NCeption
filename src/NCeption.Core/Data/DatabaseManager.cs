using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NCeption.Data
{
    public class DatabaseManager
    {
        private static readonly IDictionary<Type, IDatabaseDeployer> Databases = new Dictionary<Type, IDatabaseDeployer>();
        private DatabaseManager() { }

        static DatabaseManager()
        {
            Current = new DatabaseManager();
        }

        public static DatabaseManager Current { get; private set; }

        public void Deploy(IDatabaseDeployer databaseDeployer)
        {
            Type type = databaseDeployer.GetType();

            Console.WriteLine("Deploying database of type: '{0}'", type);
            
            if (Databases.ContainsKey(type))
            {
                Console.WriteLine("Database already deployed, skipping deployment");
                return;
            }

            databaseDeployer.Deploy(new NCeptionConfiguration());

            Databases.Add(type, databaseDeployer);

            Console.WriteLine("Database deployed");
        }

        public void Delete<TDatabaseHandler>() where TDatabaseHandler : IDatabaseDeployer
        {
            Type type = typeof(TDatabaseHandler);

            var databaseDeployer = GetDatabase(type);

            Console.WriteLine("Deleting database: '{0}'", type);

            Safely.Call(databaseDeployer, x => x.Delete());

            Databases.Remove(type);
        }

        public IDatabaseDeployer Database<TDatabaseDeployer>() where TDatabaseDeployer : IDatabaseDeployer
        {
            return GetDatabase(typeof(TDatabaseDeployer));
        }

        public void DeleteAll()
        {
            Parallel.ForEach(Databases.Values, db => Safely.Call(db, x => x.Delete()));

            Databases.Clear();
        }

        private IDatabaseDeployer GetDatabase(Type type)
        {
            if (!Databases.ContainsKey(type))
            {
                throw new DeployedDatabaseNotFoundException(type);
            }
            return Databases[type];
        }
    }
}