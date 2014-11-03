using System;
using System.IO;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.SafelyFixtures
{
    class When_safely_performing_an_action
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
            Action act = () => Safely.Call(() => { throw new Exception(); });

            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public void Should_write_out_the_exception_to_the_log()
        {
            Safely.Call(() => { throw new Exception("Fail"); });

            textWriter.Received().WriteLine(
                "Failed to call action\n{0}",
                Arg.Is<Exception>(x => x.Message == "Fail"));
        }
    }
}