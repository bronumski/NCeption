using System;
using FluentAssertions;
using NUnit.Framework;

namespace NCeption.ServiceManagerFixtures
{
    class When_starting_a_simple_service_using_generics
    {
        [Test]
        public void Should_start_the_service()
        {
            ServiceManager.Start<StartableService1>();

            StartableService1.IsRunning.Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceManager.Stop<StartableService1>();
        }
    }

    class When_starting_a_simple_service_with_a_type
    {
        [Test]
        public void Should_start_the_service()
        {
            ServiceManager.Start(typeof(StartableService1));

            StartableService1.IsRunning.Should().BeTrue();
        }

        [Test]
        public void Should_throw_an_error_if_the_type_does_not_implement_startable_service()
        {
            Action act = () => ServiceManager.Start(typeof(object));

            act
                .ShouldThrow<InvalidStartableServiceTypeException>()
                .WithMessage(
                    string.Format(
                        "Type '{0}' does not implement '{1}' and cannot be started.",
                        typeof (object),
                        typeof (IStartableService)));
        }

        [TearDown]
        public void TearDown()
        {
            ServiceManager.Stop<StartableService1>();
        }
    }
}