using System.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace NCeption.Configuration.ConfigurationExtensionsFixtures
{
    class When_updating_app_settings
    {
        [TestCase(1)]
        [TestCase("Hi")]
        [TestCase(true)]
        [TestCase(1.1)]
        public void Should_set_the_app_settings_value_of_type<T>(T value)
        {
            var configMapping = new ExeConfigurationFileMap
            {
                ExeConfigFilename = @"Configuration\TestConfig.config"
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMapping, ConfigurationUserLevel.None);

            config.AppSettings.Set("Foo", value);

            config.AppSettings.Settings["Foo"].Value.Should().Be(value.ToString());
        }
    }
}