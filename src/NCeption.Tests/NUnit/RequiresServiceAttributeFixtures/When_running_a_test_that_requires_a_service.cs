using FluentAssertions;
using NCeption.ServiceManagerFixtures;
using NUnit.Framework;

namespace NCeption.NUnit.RequiresServiceAttributeFixtures
{
    [RequireServices(typeof(StartableService1))]
    class When_running_a_test_fixture_that_requires_a_service
    {
        [Test]
        public void Should_start_up_service()
        {
            StartableService1.IsRunning.Should().BeTrue();
        }
    }
}