using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.SafelyFixtures
{
    class When_safely_shutting_down_a_service : When_safely_disposing_an_objectBase
    {
        [Test]
        public void Should_swallow_the_exception()
        {
            var foo = new StartableService();

            Action act = () => Safely.Shutdown(foo);

            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public void Should_write_out_the_exception_to_the_log()
        {
            var foo = new StartableService();

            Safely.Shutdown(foo);

            textWriter.Received().WriteLine(
                "Failed to call '{0}' on object of type '{1}'\n{2}",
                Arg.Is<Expression>(x => x.ToString() == "startable.Stop()"),
                typeof(StartableService),
                Arg.Is<Exception>(x => x.Message == "Fail"));
        }

        class StartableService : IStartableService
        {
            public void Start()
            {
                throw new NotImplementedException();
            }

            public void Stop()
            {
                throw new Exception("Fail");
            }
        }
    }
}