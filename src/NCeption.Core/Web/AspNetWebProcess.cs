using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;

namespace NCeption.Web
{
    public abstract class AspNetWebProcess : IStartableService
    {
        private bool _hasDeployed;

        public void Start()
        {
            var tempHostingFolder = CleanUpAndCreateTempHostingFolder();

            DeploySite(tempHostingFolder);

            InternalStart(tempHostingFolder.FullName);
        }

        private DirectoryInfo CleanUpAndCreateTempHostingFolder()
        {
            var folder = new DirectoryInfo(string.Format("_temp{0}", GetType().FullName));

            if (folder.Exists)
            {
                try
                {
                    folder.Delete(true);
                }
                catch (Exception)
                {
                    folder.Delete(true);
                }
            }
            folder.Create();

            return folder;
        }

        protected abstract void InternalStart(string hostingFolder);
        public abstract void Stop();

        private void DeploySite(DirectoryInfo hostingFolder)
        {
            if (_hasDeployed) return;

            using (var projectCollection = new ProjectCollection())
            {
                var globalProperty = new Dictionary<string, string>
                {
                    {"WebProjectOutputDir", hostingFolder.FullName},
                    {"OutDir", Path.Combine(hostingFolder.FullName, "bin")},
                    {"Configuration", "Release"}
                };

                var buildParameters = new BuildParameters(projectCollection)
                {
                    Loggers = new[] {new ConsoleLogger()},
                    DefaultToolsVersion = "4.0"
                };

                var buildRequestData = new BuildRequestData(WebProjectPath, globalProperty, "4.0", new []{"Build", "ResolveReferences", "_CopyWebApplication"}, null);

                var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequestData);

                if (buildResult.OverallResult != BuildResultCode.Success)
                {
                    throw new Exception(string.Format("Failed to deploy website '{0}':/n{1}", WebProjectPath, buildResult.Exception));
                }
            }

            UpdateWebConfig(hostingFolder);

            _hasDeployed = true;
        }

        private void UpdateWebConfig(DirectoryInfo hostingFolder)
        {
            var configFile = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(hostingFolder.FullName, "web.config")
            };

            var config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);

            UpdateWebConfig(config);

            config.Save();
        }

        protected virtual void UpdateWebConfig(System.Configuration.Configuration config)
        {
            
        }

        protected abstract string WebProjectPath { get; }
    }
}