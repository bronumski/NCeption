namespace NCeption
{
    public static class NCeptionGlobalSettings
    {
        public static INCeptionConfiguration Configuration
        {
            get { return new NCeptionConfiguration(); }
        }
    }
}