using FluentAssertions;
using NUnit.Framework;

namespace NCeption.ServiceManagerFixtures
{
    class When_starting_a_simple_service
    {
        [Test]
        public void Should_start_the_service()
        {
            ServiceManager.Start<FooService>();

            FooService.IsRunning.Should().BeTrue();
        }
    }
}