using NSubstitute;
using NUnit.Framework;

namespace NCeption.ServiceManagerFixtures
{
    class When_shuting_down_all_services
    {
        private IStartableService service1;
        private IStartableService service2;

        [TestFixtureSetUp]
        public void SetUp()
        {
            service1 = Substitute.For<IStartableService>();
            service2 = Substitute.For<IStartableService>();

            ServiceManager.Start(() => service1, "service1");
            ServiceManager.Start(() => service2, "service2");

            ServiceManager.StopAll();
        }

        [Test]
        public void Should_shutdown_service1()
        {
            service1.Received().Stop();
        }

        [Test]
        public void Should_shutdown_service2()
        {
            service2.Received().Stop();
        }
    }
}