using System.Net.Http;
using FluentAssertions;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Testing;
using NCeption.Nancy;
using NUnit.Framework;

namespace NCeption.NUnit.RequiresHttpClientAttributeFixtures
{
    [RequireServices(typeof(FakeEndpoint))]
    class When_depending_on_a_http_client : IRequireHttpClient<When_depending_on_a_http_client.FakeEndpoint>
    {
        [Test]
        public void Should_get_a_client_with_the_correct_base_url()
        {
            HttpClient.GetStringAsync("/").Result.Should().Be("Wibble");
        }
         
        public HttpClient HttpClient { private get; set; }

        public class FakeEndpoint : StartableNancyService
        {
            public override INancyBootstrapper Bootstrapper
            {
                get { return new ConfigurableBootstrapper(x => x.Module<FakeModule>()); }
            }
            class FakeModule : NancyModule
            {
                public FakeModule()
                {
                    Get["/"] = p => Response.AsText("Wibble");
                }
            }
        }
    }
}