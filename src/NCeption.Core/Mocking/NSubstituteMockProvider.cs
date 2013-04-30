using NSubstitute;

namespace NCeption.Mocking
{
    public class NSubstituteMockProvider : IMockProvider
    {
        public TMock Mock<TMock>() where TMock : class
        {
            return Substitute.For<TMock>();
        }
    }
}