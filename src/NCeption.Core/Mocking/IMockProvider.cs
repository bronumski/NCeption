namespace NCeption.Mocking
{
    public interface IMockProvider
    {
        TMock Mock<TMock>() where TMock : class;
    }
}