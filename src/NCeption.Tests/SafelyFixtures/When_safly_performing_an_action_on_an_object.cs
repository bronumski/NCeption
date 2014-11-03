using System;
using System.IO;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.SafelyFixtures
{
    class When_safely_performing_an_action_on_an_object
    {
        private TextWriter originalWriter;
        private TextWriter textWriter;

        [SetUp]
        public void SetUp()
        {
            textWriter = Substitute.For<TextWriter>();
            originalWriter = Console.Error;
            Console.SetError(textWriter);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetError(originalWriter);
        }

        [Test]
        public void Should_swallow_the_exception()
        {
            var foo = new Foo();

            Action act = () => Safely.Call(foo, x => x.BadMethod());

            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public void Should_write_out_the_exception_to_the_log()
        {
            var foo = new Foo();
            
            Safely.Call(foo, x => x.BadMethod());
            
            textWriter.Received().WriteLine(
                "Failed to call '{0}' on object of type '{1}'\n{2}",
                Arg.Is<Expression>(x => x.ToString() == "x.BadMethod()"),
                typeof(Foo),
                Arg.Is<Exception>(x => x.Message == "Fail"));
        }

        class Foo
        {
            public void BadMethod() { throw new Exception("Fail");}
        }
    }
}