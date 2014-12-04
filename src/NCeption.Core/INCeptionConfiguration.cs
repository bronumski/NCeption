namespace NCeption
{
    public interface INCeptionConfiguration
    {
        string TestCorrelationId { get; }
        string TestSuiteName { get; }
    }
}