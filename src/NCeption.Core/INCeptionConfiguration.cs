using System;

namespace NCeption
{
    public interface INCeptionConfiguration
    {
        string TestCorrelationId { get; }
        string TestSuiteName { get; }
        TimeSpan OrphanStaleness { get; }
    }
}