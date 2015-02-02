using Microsoft.Owin;
using Mvc4TestProject;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Mvc4TestProject
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello World");
            });
        }
    }
}