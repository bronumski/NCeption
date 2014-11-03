using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.ServiceManagerFixtures
{
    class When_starting_a_service_that_has_a_dependency
    {
        [Test]
        public void Should_start_the_service_with_a_dependancy_instance()
        {
            ServiceManager.RegisterInstance(Substitute.For<IDependency>());

            ServiceManager.Start<Wibble>();

            FooService.IsRunning.Should().BeTrue();
        }

        [Test]
        public void Should_start_the_service_with_a_dependancy_type()
        {
            ServiceManager.RegisterType<IDependency, Dependancy>();

            ServiceManager.Start<Wibble>();

            FooService.IsRunning.Should().BeTrue();
        }
        
        class Wibble : FooService
        {
            public Wibble(IDependency dependency)
            {
                
            }
        }

        class Dependancy : IDependency { }
    }
}