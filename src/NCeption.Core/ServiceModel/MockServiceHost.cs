using System;
using System.ServiceModel;

namespace NCeption.ServiceModel
{
    public class MockServiceHost<TMockService> : ServiceHost, IStartableService where TMockService : class
    {
        public MockServiceHost(TMockService mock, Uri uri)
            : base(mock, new[] { uri })
        {
            ServiceMock = mock;
        }

        public TMockService ServiceMock { get; private set; }

        void IStartableService.Start()
        {
            Open();
        }

        void IStartableService.Stop()
        {
            Close();
        }
    }
}