using System;

namespace NCeption.Mocking
{
    public class SimpleMockProvider : IMockProvider
    {
        private readonly Func<Type, object> mockFactory;

        public SimpleMockProvider(Func<Type, object> mockFactory)
        {
            this.mockFactory = mockFactory;
        }

        public TMock Mock<TMock>() where TMock : class
        {
            return (TMock)mockFactory(typeof (TMock));
        }
    }
}