using CassiniDev;

namespace NCeption.Web
{
    public abstract class CassiniDevStartableService : AspNetWebProcess
    {
        private Server cassiniDevServer;
        protected override void InternalStart(string hostingFolder)
        {
            var portNumber = UriManager.GetUriForKey(GetType().FullName).Port;

            cassiniDevServer = new Server(portNumber, hostingFolder);

            cassiniDevServer.Start();
        }

        public override void Stop()
        {
            Safely.Call(cassiniDevServer, server => server.ShutDown());
            Safely.Dispose(cassiniDevServer);
        }
    }
}