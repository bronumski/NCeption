using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using CassiniDev;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Logging;

namespace NCeption
{
    public abstract class CassiniDevWebProcess : IStartableService
    {
        private readonly Server cassiniDevServer;
        private readonly DirectoryInfo websiteDirectory;
        
        protected CassiniDevWebProcess(string webAppProjectPath, string websiteKeyName, string tempHostingPath = null)
        {
            websiteDirectory = new DirectoryInfo(tempHostingPath ?? GetHostingFolderPath(websiteKeyName));

            DeployWebProjectToTempFolder(webAppProjectPath, websiteDirectory, websiteKeyName);

            var uri = UriManager.GetUriForKey(websiteKeyName);

            cassiniDevServer = new Server(uri.Port, uri.PathAndQuery, websiteDirectory.FullName, IPAddress.Loopback, uri.Host);
        }

        public void Start()
        {
            SaveWebConfig(GetWebConfigFileName(websiteDirectory));

            cassiniDevServer.Start();
        }

        public void Stop()
        {
            Safely.Dispose(cassiniDevServer);
        }

        protected abstract void UpdateWebConfig(Configuration webConfig);

        protected virtual void SaveWebConfig(string webConfigFileName)
        {
            var configurationFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = webConfigFileName
            };

            var webConfig = ConfigurationManager.OpenMappedExeConfiguration(configurationFileMap, ConfigurationUserLevel.None);

            UpdateWebConfig(webConfig);

            webConfig.Save(ConfigurationSaveMode.Full);
        }

        private static string GetWebConfigFileName(DirectoryInfo websiteDirectory)
        {
            return websiteDirectory.GetFiles("web.config").First().FullName;
        }

        private void DeployWebProjectToTempFolder(string webAppProjectPath, DirectoryInfo webSiteDirectory, string websiteName)
        {
            CleanAndCreateWebSiteDirectory(webSiteDirectory);

            var properties = new Dictionary<string, string>
			                 	{
			                 		{"Configuration", "Release"},
			                 		{"WebProjectOutputDir", webSiteDirectory.FullName},
			                 		{"OutDir", Path.Combine(webSiteDirectory.FullName, "bin\\")}
			                 	};
            using (var engine = new ProjectCollection(properties))
            {
                //var fileLogger = new FileLogger { Parameters = string.Format(@"logfile={0}", Path.Combine(NCeptionGlobalSettings.Configuration.LoggingFolder, string.Format("Deploy-{0}-Website.txt", websiteName))) };
                var buildResult = engine
                    .LoadProject(webAppProjectPath, "4.0")
                    .Build(new[] { "Build", "ResolveReferences", "_CopyWebApplication" }, new[] { new ConsoleLogger() });

                if (!buildResult)
                {
                    throw new Exception(string.Format("Failed to deploy '{0}' website", websiteName));
                }
            }
        }

        private static void CleanAndCreateWebSiteDirectory(DirectoryInfo webSiteDirectory)
        {
            if (webSiteDirectory.Exists)
            {
                webSiteDirectory.Delete(recursive: true);
            }

            webSiteDirectory.Create();
        }

        private static string GetHostingFolderPath(string websiteName)
        {
            return Path.Combine(NCeptionGlobalSettings.Configuration.TempHostingFolder, websiteName);
        }
    }
}