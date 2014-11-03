using FluentAssertions;
using NUnit.Framework;

namespace NCeption.ServiceManagerFixtures
{
    class When_stoping_a_simple_service
    {
        [Test]
        public void Should_stop_the_service()
        {
            ServiceManager.Start<FooService>();

            ServiceManager.Stop<FooService>();

            FooService.IsRunning.Should().BeFalse();
        }
    }
}