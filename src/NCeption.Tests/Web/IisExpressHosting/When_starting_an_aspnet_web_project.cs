using System.Net;
using System.Net.Http;
using FluentAssertions;
using NCeption.NUnit;
using NUnit.Framework;

namespace NCeption.Web.IisExpressHosting
{
    [RequireServices(typeof(Mvc4TestProject))]
    class When_starting_an_aspnet_web_project : IRequireHttpClient<When_starting_an_aspnet_web_project.Mvc4TestProject>
    {
        [Test]
        public void Should_deploy_website_and_run_it()
        {
            HttpClient.GetAsync("/Home/Index").Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public HttpClient HttpClient { set; private get; }

        class Mvc4TestProject : IisExpressStartableService
        {
            protected override string WebProjectPath
            {
                get { return @"..\..\..\TestProjects\Mvc4TestProject\Mvc4TestProject.csproj"; }
            }
        }
    }
}