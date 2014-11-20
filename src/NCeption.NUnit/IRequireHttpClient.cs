using System.Net.Http;

namespace NCeption.NUnit
{
    public interface IRequireHttpClient
    {
        HttpClient HttpClient { set; }
    }

    [RequireHttpClient]
    public interface IRequireHttpClient<T> : IRequireHttpClient
    {
    }
}