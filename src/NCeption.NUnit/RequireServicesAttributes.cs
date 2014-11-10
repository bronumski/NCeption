using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NCeption.NUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireServicesAttribute : Attribute, ITestAction
    {
        private readonly Type[] services;

        public RequireServicesAttribute(params Type[] services)
        {
            this.services = services;
        }

        public void BeforeTest(TestDetails testDetails)
        {
            Parallel.ForEach(services, service => ServiceManager.Start(service));
        }

        public void AfterTest(TestDetails testDetails)
        {
            Parallel.ForEach(services, service => ServiceManager.Stop(service));
        }

        public ActionTargets Targets { get { return ActionTargets.Default; } }
    }
}