namespace NCeption.ServiceManagerFixtures
{
    internal class FooService : IStartableService
    {
        public static bool IsRunning { get; private set; }

        public FooService()
        {
            IsRunning = false;
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }

    public interface IDependency { }
}