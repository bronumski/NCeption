using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace NCeption
{
    public class NCeptionConfiguration : INCeptionConfiguration
    {
        private readonly NameValueCollection nameValueCollection;

        internal NCeptionConfiguration()
        {
            nameValueCollection = ConfigurationManager.AppSettings;
        }

        public string LoggingFolder
        {
            get
            {
                return nameValueCollection.AllKeys.Contains("LoggingFolder") ? nameValueCollection["LoggingFolder"] : "NCeptionLogs";
            }
        }

        public string TempHostingFolder
        {
            get
            {
                return nameValueCollection.AllKeys.Contains("TempHostingFolder") ? nameValueCollection["TempHostingFolder"] : "TempHosting";
            }
        }
    }
}