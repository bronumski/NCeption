using System;

namespace NCeption
{
    public class NCeptionConfiguration : INCeptionConfiguration
    {
        private static readonly string testCorrelationId = Guid.NewGuid().ToString("N");
        private const string testSuiteName = "TestSuite";
        private static readonly TimeSpan orphanStaleness = TimeSpan.FromDays(1);

        public string TestCorrelationId { get { return testCorrelationId; } }
        public string TestSuiteName { get { return testSuiteName; } }
        public TimeSpan OrphanStaleness { get { return orphanStaleness; } }
    }
}