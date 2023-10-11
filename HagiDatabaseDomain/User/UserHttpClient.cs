using HagiDatabaseDomain;

namespace HagiDomain
{
    public class UserHttpClient : IDisposable
    {
        private HttpClient _httpClient;

        public UserHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }





        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}