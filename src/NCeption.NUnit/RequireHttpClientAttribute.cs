using System;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;

namespace NCeption.NUnit
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    internal class RequireHttpClientAttribute : Attribute, ITestAction
    {
        private static readonly Type requiresHttpClientGenericDefType = typeof(IRequireHttpClient<>);
        private const string errorFormat = "The test fixture '{0}' is using the attrute '{1}' incorrectly.\nInstead of using the attribute '{1}' directly, implement the interface '{2}' in your test fixture";
        private HttpClient httpClient;

        public void BeforeTest(TestDetails testDetails)
        {
            var requiresHttpClientFixture = testDetails.Fixture as IRequireHttpClient;

            var serviceType = GetGenericType(requiresHttpClientFixture);

            if (serviceType == null)
            {
                throw new InvalidOperationException(string.Format(errorFormat, testDetails.Fixture.GetType(), GetType(), requiresHttpClientGenericDefType));
            }

            httpClient = new HttpClient { BaseAddress = UriManager.GetUriForKey(serviceType.FullName) };

            requiresHttpClientFixture.HttpClient = httpClient;
        }

        private Type GetGenericType(IRequireHttpClient requiresHttpClientFixture)
        {
            if (requiresHttpClientFixture == null) return null;

            var requiresHttpClientGenericInterface = requiresHttpClientFixture.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == requiresHttpClientGenericDefType);

            return requiresHttpClientGenericInterface == null ? null : requiresHttpClientGenericInterface.GetGenericArguments().First();
        }

        public void AfterTest(TestDetails testDetails)
        {
            Safely.Dispose(httpClient);
        }

        public ActionTargets Targets { get { return ActionTargets.Test; } }
    }
}