using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NCeption
{
    public static class ServiceManager
    {
        private static readonly object lockObject = new object();
        private static readonly IDictionary<string, IStartableService> runningServices = new Dictionary<string, IStartableService>();

        public static void Start<TService>(string key = null) where TService : IStartableService, new()
        {
            Start(() => new TService(), key);
        }

        public static void Start<TService>(Func<TService> serviceFactory, string key = null) where TService : IStartableService
        {
            string serviceKey = key ?? typeof(TService).FullName;

            lock (lockObject)
            {
                if (runningServices.ContainsKey(serviceKey))
                {
                    return;
                }

                var service = serviceFactory();

                service.Start();

                runningServices.Add(serviceKey, service);
            }
        }

        public static void Stop<T>(string key = null)
        {
            var serviceKey = key ?? typeof (T).FullName;

            lock (lockObject)
            {
                if (runningServices.ContainsKey(serviceKey))
                {
                    var service = runningServices[serviceKey];

                    Safely.Call(service, x => x.Stop());

                    runningServices.Remove(serviceKey);
                }
            }
        }

        public static void StopAll()
        {
            lock (lockObject)
            {
                Parallel.ForEach(runningServices, x => Safely.Call(x.Value, y => y.Stop()));
            }
        }
    }
}