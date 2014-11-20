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
            host = new NancyHost(Bootstrapper, new HostConfiguration{UrlReservations = new UrlReservations { CreateAutomatically = true }}, uri);

            host.Start();
        }

        public abstract INancyBootstrapper Bootstrapper { get; }

        public void Stop()
        {
            Safely.Dispose(host);
        }
    }
}