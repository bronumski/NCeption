using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyIoC;

namespace NCeption
{
    public static class ServiceManager
    {
        private static readonly object lockObject = new object();
        private static readonly IDictionary<string, IStartableService> runningServices = new Dictionary<string, IStartableService>();
        private static readonly TinyIoCContainer container;

        static ServiceManager()
        {
            container = new TinyIoCContainer();
        }

        public static void RegisterType<TService, TImplementation>() where TImplementation : class
        {
            container.Register(typeof (TService), typeof (TImplementation));
        }
        public static void RegisterInstance<TComponent>(TComponent instance) where TComponent : class
        {
            container.Register(instance);
        }

        public static void Start(Type serviceType, string key = null)
        {
            if (serviceType.GetInterfaces().Any(x => x == typeof (IStartableService)) == false)
            {
                throw new InvalidStartableServiceTypeException(
                    string.Format("Type '{0}' does not implement '{1}' and cannot be started.", serviceType,
                        typeof (IStartableService)));
            }

            Start(() => (IStartableService) container.Resolve(serviceType), key);
        }

        public static void Start<TService>(string key = null) where TService : class, IStartableService
        {
            Start(container.Resolve<TService>, key);
        }

        public static void Start<TService>(Func<TService> serviceFactory, string key = null) where TService : IStartableService
        {
            Start(() => (IStartableService)serviceFactory(), key);
        }

        private static void Start(Func<IStartableService> serviceFactory, string key)
        {
            lock (lockObject)
            {
                var service = serviceFactory();

                Type serviceType = service.GetType();

                if (IsRunning(serviceType, key)) return;

                service.Start();

                runningServices.Add(GenerateFullKey(serviceType, key), service);
            }
        }

        private static bool IsRunning<TServiceType>(string key)
        {
            return IsRunning(typeof (TServiceType), key);
        }

        private static bool IsRunning(Type serviceType, string key)
        {
            return runningServices.ContainsKey(GenerateFullKey(serviceType, key));
        }

        private static string GenerateFullKey<TService>(string key)
        {
            return GenerateFullKey(typeof (TService), key);
        }

        private static string GenerateFullKey(Type serviceType, string key)
        {
            return serviceType.FullName + key;
        }

        public static void Stop<T>(string key = null)
        {
            Stop(typeof(T), key);
        }

        public static void Stop(Type serviceType, string key = null)
        {
            var serviceKey = GenerateFullKey(serviceType, key);

            lock (lockObject)
            {
                if (! IsRunning(serviceType, key)) return;

                var service = runningServices[serviceKey];

                Safely.Call(service, x => x.Stop());

                runningServices.Remove(serviceKey);
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