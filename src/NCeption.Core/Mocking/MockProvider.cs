namespace NCeption.Mocking
{
    public static class MockProvider
    {
        public static IMockProvider mockProvider = new NSubstituteMockProvider();

        public static TMock CreateMock<TMock>() where TMock : class
        {
            return mockProvider.Mock<TMock>();
        }
    }
}