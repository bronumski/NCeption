using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace NCeption.Nancy
{
    public abstract class StartableNancyService : IStartableService
    {
        private NancyHost host;

        public void Start()
        {
            var uri = UriManager.GetUriForKey(GetType().ToString());
            host = new NancyHost(uri, Bootstrapper);

            host.Start();
        }

        public abstract INancyBootstrapper Bootstrapper { get; }

        public void Stop()
        {
            Safely.Dispose(host);
        }
    }
}