using System;

namespace NCeption
{
    public class NCeptionConfiguration : INCeptionConfiguration
    {
        private static readonly string testCorrelationId = Guid.NewGuid().ToString("N");
        private const string testSuiteName = "TestSuite";

        static NCeptionConfiguration()
        {
        }

        public string TestCorrelationId { get { return testCorrelationId; } }
        public string TestSuiteName { get { return testSuiteName; } }
    }
}