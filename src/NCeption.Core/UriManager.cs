using System;
using System.Collections.Generic;

namespace NCeption
{
    public static class UriManager
    {
        private static readonly object lockObject = new object();
        private static readonly IDictionary<string, Uri> UriStore = new Dictionary<string, Uri>();

        public static Uri GetUriForService<TService>()
        {
            var serviceFullName = typeof (TService).FullName;

            return GetUriForKey(serviceFullName);
        }

        public static Uri GetUriForKey(string key)
        {
            return GetUriFromStore(key) ?? CreateUriAndStore(key);
        }

        private static Uri GetUriFromStore(string key)
        {
            lock (lockObject)
            {
                return UriStore.ContainsKey(key) ? UriStore[key] : null;
            }
        }

        private static Uri CreateUriAndStore(string key)
        {
            lock (lockObject)
            {
                var uriBuilder = new UriBuilder(Uri.UriSchemeHttp, "localhost", TcpPort.GetNextFreePort(), key);

                UriStore.Add(key, uriBuilder.Uri);

                return uriBuilder.Uri;
            }
        }

        public static void Reset()
        {
            lock (lockObject)
            {
                UriStore.Clear();
            }
        }
    }
}