using System;
using System.ServiceModel;
using FluentAssertions;
using NCeption.Net;
using NSubstitute;
using NUnit.Framework;

namespace NCeption.ServiceModel
{
    [TestFixture]
    class When_getting_mocked_service_host
    {
        private MockServiceHost<IFooService> mockServiceHost;
        private Uri baseAddress;
        private ChannelFactory<IFooService> channelFactory;

        [SetUp]
        public void SetUp()
        {
            baseAddress = new Uri("http://localhost:" + TcpPort.GetNextFreePort());

            mockServiceHost = MockServiceHostFactory.GenerateMockServiceHost<IFooService>(baseAddress, "Foo");

            mockServiceHost.Open();

            channelFactory = new ChannelFactory<IFooService>(new BasicHttpBinding(), new EndpointAddress(new Uri(baseAddress, "Foo")));
        }

        [Test]
        public void Should_return_a_service_host_with_the_mocked_service()
        {
            mockServiceHost.ServiceMock.Should().NotBeNull();
        }

        [Test]
        public void Should_be_able_to_start_service_host()
        {
            mockServiceHost.State.Should().Be(CommunicationState.Opened);
        }

        [Test]
        public void Should_be_able_to_call_mocked_service_method()
        {
            mockServiceHost.ServiceMock.Bar().Returns(101);

            var channel = channelFactory.CreateChannel();

            channel.Bar().Should().Be(101);
        }

        [Test]
        public void Should_return_exception_faults()
        {
            mockServiceHost.ServiceMock.Bar().Returns(x => { throw new Exception("FooBar"); });

            var channel = channelFactory.CreateChannel();

            Action act = () => channel.Bar();

            act.ShouldThrow<Exception>().And.Message.Should().Be("FooBar");
        }

        [TearDown]
        public void TearDown()
        {
            channelFactory.Close();
            
            ((IDisposable)channelFactory).Dispose();

            mockServiceHost.Close();
        }
    }

    [ServiceContract]
    public interface IFooService
    {
        [OperationContract]
        int Bar();
    }
}