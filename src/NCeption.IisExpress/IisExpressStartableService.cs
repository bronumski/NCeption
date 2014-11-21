namespace NCeption.Web
{
    public abstract class IisExpressStartableService : AspNetWebProcess
    {
        private IISExpress iisExpress;

        protected override void InternalStart(string hostingFolder)
        {
            var portNumber = UriManager.GetUriForKey(GetType().FullName).Port;

            iisExpress = IISExpress.Start(hostingFolder, portNumber);
        }

        public override void Stop()
        {
            Safely.Dispose(iisExpress);
        }
    }
}