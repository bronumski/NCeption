using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.SafelyFixtures
{
    internal class When_safely_disposing_an_objectBase
    {
        private TextWriter originalWriter;
        protected TextWriter textWriter;

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
    }
}