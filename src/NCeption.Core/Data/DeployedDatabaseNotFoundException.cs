using System;

namespace NCeption.Data
{
    public class DeployedDatabaseNotFoundException : Exception
    {
        public DeployedDatabaseNotFoundException(Type type)
            : base(string.Format("Could not delete deployed database '{0}'", type))
        {
            
        }
    }
}