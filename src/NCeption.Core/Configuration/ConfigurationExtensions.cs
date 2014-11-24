using System.Configuration;

namespace NCeption.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void Set<T>(this AppSettingsSection appSettings, string key, T value)
        {
            appSettings.Settings.Add(key, value.ToString());
        }
    }
}