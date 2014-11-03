using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.SafelyFixtures
{
    class When_safely_disposing_an_object : When_safely_disposing_an_objectBase
    {
        [Test]
        public void Should_swallow_the_exception()
        {
            var foo = new Disposable();

            Action act = () => Safely.Dispose(foo);

            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public void Should_write_out_the_exception_to_the_log()
        {
            var foo = new Disposable();

            Safely.Dispose(foo);

            textWriter.Received().WriteLine(
                "Failed to call '{0}' on object of type '{1}'\n{2}",
                Arg.Is<Expression>(x => x.ToString() == "disposable.Dispose()"),
                typeof(Disposable),
                Arg.Is<Exception>(x => x.Message == "Fail"));
        }

        class Disposable : IDisposable
        {
            public void Dispose()
            {
                throw new Exception("Fail");
            }
        }

    }
}