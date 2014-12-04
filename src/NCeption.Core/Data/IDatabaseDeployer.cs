namespace NCeption.Data
{
    public interface IDatabaseDeployer
    {
        void Deploy(INCeptionConfiguration configuration);

        void Delete();
    }
}