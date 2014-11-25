using System.Net;
using System.Net.Http;
using FluentAssertions;
using NCeption.NUnit;
using NCeption.Owin;
using NUnit.Framework;
using Owin;

namespace NCeption.Web.OwinHosting
{
    [RequireServices(typeof(OwinTestProject))]
    class When_starting_an_owin_web_project : IRequireHttpClient<When_starting_an_owin_web_project.OwinTestProject>
    {
        [Test]
        public void Should_deploy_website_and_run_it()
        {
            HttpClient.GetAsync("/Home/Index").Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public HttpClient HttpClient { set; private get; }

        class OwinTestProject : OwinStartableService<OwinTestStartup>
        {
        }

        class OwinTestStartup
        {
            public void Configuration(IAppBuilder app)
            {
                app.Run(context => context.Response.WriteAsync("Hello, world"));
            }
        }
    }
}