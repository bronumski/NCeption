using System.Net.Http;
using FluentAssertions;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Testing;
using NUnit.Framework;

namespace NCeption.Nancy
{
    class When_starting_nancy_module
    {
        [Test]
        public async void Should_start_up_Nancy_self_host()
        {
            ServiceManager.Start<TestStartableNancyService>();

            using (var httpClient = new HttpClient { BaseAddress = UriManager.GetUriForService<TestStartableNancyService>()})
            {
                var wibbleResponse = await httpClient.GetAsync("wibble");
                var wibble = await wibbleResponse.Content.ReadAsAsync<Wibble>();

                wibble.Should().NotBeNull();
            }
        }

        [TearDown]
        public void TearDown()
        {
            ServiceManager.StopAll();
        }

        class TestNancyModule : NancyModule
        {
            public TestNancyModule()
            {
                Get["Wibble"] = p => Response.AsJson(new Wibble());
            }
        }

        class TestStartableNancyService : StartableNancyService
        {
            public override INancyBootstrapper Bootstrapper
            {
                get { return new ConfigurableBootstrapper(c => c.Module<TestNancyModule>());}
            }
        }
        internal class Wibble {  }
    }
}