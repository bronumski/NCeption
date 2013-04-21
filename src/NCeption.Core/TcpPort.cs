using System.Net;
using System.Net.Sockets;

namespace NCeption
{
    public static class TcpPort
    {
        public static int GetNextFreePort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                l.Start();

                return ((IPEndPoint)l.LocalEndpoint).Port;
            }
            finally
            {
                l.Stop();
            }
        }
    }
}