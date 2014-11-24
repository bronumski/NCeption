using System;
using Microsoft.Owin.Hosting;

namespace NCeption.Owin
{
    public class OwinStartableService<TStartUp> : IStartableService
    {
        private IDisposable owinTestServer;

        public void Start()
        {
            owinTestServer = WebApp.Start<TStartUp>(UriManager.GetUriForKey(GetType().FullName).ToString());
        }

        public void Stop()
        {
            Safely.Dispose(owinTestServer);
        }
    }
}
