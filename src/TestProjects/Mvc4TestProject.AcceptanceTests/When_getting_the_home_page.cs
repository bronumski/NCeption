using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NCeption;
using NUnit.Framework;

namespace Mvc4TestProject.AcceptanceTests
{
    [TestFixture]
    class When_getting_the_home_page
    {
        private HttpClient httpClient;
        private Task<HttpResponseMessage> response;

        [SetUp]
        public void SetUp()
        {
            ServiceManager.Start(() => new Mvc4TestProjectWebsiteStarter(@"..\..\..\Mvc4TestProject\Mvc4TestProject.csproj", "Mvc4TestProject"));
        }

        [TearDown]
        public void TearDown()
        {
            ServiceManager.Stop<Mvc4TestProjectWebsiteStarter>();
        }

        [Test]
        public void SimpleWebRequest()
        {
            new StoryQ.Story("Home Page")
                .InOrderTo("Access the website")
                .AsA("Site visitor")
                .IWant("To be able to view the home page")
                .WithScenario("Simple web request")
                    .Given(IHaveOpenedMyWebBrowser)
                    .When(INavigateToTheHomePage)
                    .Then(IShouldSeeTheHomePage)
                .Execute();
        }

        private void IHaveOpenedMyWebBrowser()
        {
            httpClient = new HttpClient();
        }

        private void INavigateToTheHomePage()
        {
            httpClient.BaseAddress = UriManager.GetUriForKey("Mvc4TestProject");
            response = httpClient.GetAsync("Home");
        }

        private void IShouldSeeTheHomePage()
        {
            response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    internal class Mvc4TestProjectWebsiteStarter : CassiniDevWebProcess
    {
        public Mvc4TestProjectWebsiteStarter(string mvc4testprojectMvc4testprojectCsproj, string mvc4testproject) : base(mvc4testprojectMvc4testprojectCsproj, mvc4testproject)
        {
    
        }

        protected override void UpdateWebConfig(Configuration webConfig)
        {
            
        }
    }
}