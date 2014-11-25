using System;

namespace NCeption.Mocking
{
    public static class MockProvider
    {
        private static IMockProvider MockProviderFactory;

        public static TMock CreateMock<TMock>() where TMock : class
        {
            if (MockProviderFactory == null)
            {
                throw new Exception("Mock provider has not been set yet.");
            }
            return MockProviderFactory.Mock<TMock>();
        }

        public static void Initialize(IMockProvider mockProvider)
        {
            MockProviderFactory = mockProvider;
        }
    }
}