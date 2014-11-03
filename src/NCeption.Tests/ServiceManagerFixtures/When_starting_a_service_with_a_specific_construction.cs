using FluentAssertions;
using NUnit.Framework;

namespace NCeption.ServiceManagerFixtures
{
    class When_starting_a_service_with_a_specific_construction
    {
        [Test]
        public void Should_start_the_service()
        {
            ServiceManager.Start(() => new StartableService1());

            StartableService1.IsRunning.Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceManager.Stop<StartableService1>();
        }
    }
}