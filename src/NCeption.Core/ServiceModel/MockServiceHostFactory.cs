using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using NCeption.Mocking;

namespace NCeption.ServiceModel
{
    public static class MockServiceHostFactory
    {
        public static MockServiceHost<TMockService> GenerateMockServiceHost<TMockService>(Uri baseAddress, string endpointAddress) where TMockService : class
        {
            var mock = MockProvider.CreateMock<TMockService>();

            var serviceHost = new MockServiceHost<TMockService>(mock, baseAddress);

            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;

            serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>().InstanceContextMode = InstanceContextMode.Single;

            serviceHost.AddServiceEndpoint(typeof(TMockService), new BasicHttpBinding(), endpointAddress);

            return serviceHost;
        }
    }
}