namespace NCeption.ServiceManagerFixtures
{
    internal class StartableService1 : IStartableService
    {
        public static bool IsRunning { get; private set; }

        public StartableService1()
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

    class StartableService2 : StartableService1
    {
         
    }

    public interface IDependency { }
}