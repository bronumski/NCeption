using System;
using System.Net;
using System.Net.Sockets;
using FluentAssertions;
using NUnit.Framework;

namespace NCeption.Net.TcpPortFixtures.GetNextFreePort
{
    [TestFixture]
    class When_getting_next_free_port
    {
        private int result;

        [SetUp]
        public void SetUp()
        {
            result = TcpPort.GetNextFreePort();
        }

        [Test]
        public void Should_get_a_port_number()
        {
            result.Should().BeGreaterThan(0);
        }

        [Test]
        public void Should_be_able_to_open_a_socket_with_the_port_number()
        {
            var l = new TcpListener(IPAddress.Loopback, result);

            Action act = l.Start;
            
            act.ShouldNotThrow<Exception>();
            
            l.Stop();
        }
    }
}